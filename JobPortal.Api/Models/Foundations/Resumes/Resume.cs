namespace JobPortal.Api.Models.Foundations.Resumes
{
    public class Resume
    {
        public int Id { get; set; }
        public int SeekerId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Skills { get; set; } = null!;
        public string Experience { get; set; } = null!;
        public string? Location { get; set; }
        public string? FileUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
