namespace JobPortal.Api.Models
{
    public class Application
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int JobPostId { get; set; }
        public string ResumePath { get; set; } = null!;
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
        public JobPost JobPost { get; set; } = null!;
    }
}
