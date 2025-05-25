namespace JobPortalAPI.DTOs
{
    public class TokenRequestDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
