namespace JobPortal.Api.DTOs.Job
{
    public class JobSearchDto
    {
        public string? Title { get; set; }
        public string? Location { get; set; }
        public JobType? JobType { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
