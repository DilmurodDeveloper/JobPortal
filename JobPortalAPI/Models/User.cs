namespace JobPortalAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; } 
        public string Role { get; set; } = "JobSeeker";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
