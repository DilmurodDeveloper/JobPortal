namespace JobPortal.Api.DTOs.Job
{
    public class JobUpdateDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Company { get; set; } = null!;
        public string Location { get; set; } = null!;
        public decimal Salary { get; set; }
        public JobType JobType { get; set; }
    }
}
