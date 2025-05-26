namespace JobPortal.Api.DTOs.Application
{
    public class ApplicationDto
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int SeekerId { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public string JobTitle { get; set; } = null!;
        public string SeekerName { get; set; } = null!;
        public string? SeekerPhoneNumber { get; set; } 
    }
}
