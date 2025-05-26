namespace JobPortal.Api.Services.Foundations.Auth
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task<TokenResponseDto> RefreshTokenAsync(TokenRequestDto tokenRequest);
        int? GetUserIdFromClaims(ClaimsPrincipal user);
    }
}
