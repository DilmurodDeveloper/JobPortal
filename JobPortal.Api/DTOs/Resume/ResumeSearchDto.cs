namespace JobPortal.Api.DTOs.Resume
{
    public class ResumeSearchDto
    {
        public string? Skill { get; set; }
        public string? Location { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
