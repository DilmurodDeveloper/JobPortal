using Microsoft.EntityFrameworkCore;

namespace JobPortal.Api.Services.Foundations.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IEmailService _emailService;
        private readonly JwtService _jwt;

        public AuthService(ApplicationDbContext db, IPasswordHasher<User> hasher, JwtService jwt, IEmailService emailService)
        {
            _db = db;
            _hasher = hasher;
            _jwt = jwt;
            _emailService = emailService;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email already registered");

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Role = dto.Role ?? Role.User,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            var profile = new UserProfile
            {
                UserId = user.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.UserProfiles.AddAsync(profile);
            await _db.SaveChangesAsync();

            return _jwt.GenerateToken(user);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email)
                       ?? throw new Exception("User not found");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash!, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Incorrect password");

            return _jwt.GenerateToken(user);
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(TokenRequestDto tokenRequest)
        {
            var principal = _jwt.GetPrincipalFromExpiredToken(tokenRequest.AccessToken)
                            ?? throw new Exception("Invalid access token");

            var email = principal.Identity?.Name;
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email)
                       ?? throw new Exception("User not found");

            if (user.RefreshToken != tokenRequest.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new Exception("Invalid refresh token");

            var newAccessToken = _jwt.GenerateToken(user);
            var newRefreshToken = _jwt.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _db.SaveChangesAsync();

            return new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task LogoutAsync(int userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _db.SaveChangesAsync();
            }
        }

        public async Task SendResetPasswordTokenAsync(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return;

            var token = Guid.NewGuid().ToString(); 
            await _emailService.SendAsync(email, "Reset your password", $"Reset token: {token}");
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) throw new Exception("User not found");


            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _db.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
                throw new Exception("Current password is incorrect");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _db.SaveChangesAsync();
        }

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return;
            }

            var resetToken = Guid.NewGuid().ToString();

            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1); 

            await _db.SaveChangesAsync();

            var resetLink = $"https://yourfrontend.com/reset-password?token={resetToken}";

            var subject = "Password Reset Request";
            var body = $"Click this link to reset your password: {resetLink}";

            await _emailService.SendAsync(email, subject, body);
        }

        public int? GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return id != null ? int.Parse(id) : null;
        }
    }
}
