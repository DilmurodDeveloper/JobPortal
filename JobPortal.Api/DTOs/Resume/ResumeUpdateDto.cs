namespace JobPortal.Api.DTOs.Resume
{
    public class ResumeUpdateDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Skills { get; set; } = null!;
        public string Experience { get; set; } = null!;
    }
}
