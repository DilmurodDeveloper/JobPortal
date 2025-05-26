namespace JobPortal.Api.Services.Foundations.Auth
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task<TokenResponseDto> RefreshTokenAsync(TokenRequestDto tokenRequest);
        int? GetUserIdFromClaims(ClaimsPrincipal user);
        Task LogoutAsync(int userId);
        Task ForgotPasswordAsync(string email);
        Task SendResetPasswordTokenAsync(string email);
        Task ResetPasswordAsync(ResetPasswordDto dto);
        Task ChangePasswordAsync(int userId, ChangePasswordDto dto);
    }
}
