namespace JobPortal.Api.DTOs.Job
{
    public class JobReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Company { get; set; } = null!;
        public string Location { get; set; } = null!;
        public decimal Salary { get; set; }
        public JobType JobType { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
