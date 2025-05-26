namespace JobPortal.Api.DTOs.Admin
{
    public class StatisticsDto
    {
        public int TotalUsers { get; set; }
        public int BlockedUsers { get; set; }
        public int AdminCount { get; set; }
        public int EmployerCount { get; set; }
        public int UserCount { get; set; }
    }
}
