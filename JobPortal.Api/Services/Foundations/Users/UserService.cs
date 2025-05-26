namespace JobPortal.Api.Services.Foundations.Users
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public UserService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<User> GetMyUserAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return user;
        }

        public async Task UpdateMyUserAsync(int userId, User updatedUser)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            user.FirstName = updatedUser.FirstName ?? user.FirstName;
            user.LastName = updatedUser.LastName ?? user.LastName;
            user.Email = updatedUser.Email ?? user.Email;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteMyAccountAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            user.Status = UserStatus.Blocked;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> PatchUserAsync(int userId, JsonPatchDocument<User> patchDoc)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            patchDoc.ApplyTo(user);

            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<string> UploadAvatarAsync(IFormFile file, int userId)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Unsupported file type.");

            var fileName = $"avatar_{userId}{extension}";
            var avatarFolder = Path.Combine(_env.WebRootPath, "avatars");

            if (!Directory.Exists(avatarFolder))
                Directory.CreateDirectory(avatarFolder);

            var filePath = Path.Combine(avatarFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.AvatarUrl = $"/avatars/{fileName}";
                user.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return $"/avatars/{fileName}";
        }
    }
}
