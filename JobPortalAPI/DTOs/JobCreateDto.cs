using JobPortalAPI.Enums;

namespace JobPortalAPI.DTOs
{
    public class JobCreateDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Company { get; set; } = null!;
        public string Location { get; set; } = null!;
        public decimal Salary { get; set; }
        public JobType JobType { get; set; } 
    }
}