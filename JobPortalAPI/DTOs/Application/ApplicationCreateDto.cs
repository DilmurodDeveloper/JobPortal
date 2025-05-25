namespace JobPortalAPI.DTOs.Application
{
    public class ApplicationCreateDto
    {
        public int JobPostId { get; set; }
        public string ResumePath { get; set; } = null!;
    }
}
