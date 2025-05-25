namespace JobPortalAPI.DTOs.Token
{
    public class TokenRequestDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
