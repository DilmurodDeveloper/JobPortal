namespace JobPortal.Api.DTOs.Resumes
{
    public class ResumeSearchDto
    {
        public string? Skill { get; set; }
        public string? Location { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
