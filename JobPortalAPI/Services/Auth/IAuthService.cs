using System.Security.Claims;
using JobPortalAPI.DTOs.Auth;
using JobPortalAPI.DTOs.Token;
using JobPortalAPI.Models;

namespace JobPortalAPI.Services.Auth
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task<TokenResponseDto> RefreshTokenAsync(TokenRequestDto tokenRequest);
        int? GetUserIdFromClaims(ClaimsPrincipal user);
    }
}
