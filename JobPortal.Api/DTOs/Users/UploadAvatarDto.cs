namespace JobPortal.Api.DTOs.Users
{
    public class UploadAvatarDto
    {
        [Required]
        public IFormFile? File { get; set; }
    }
}
