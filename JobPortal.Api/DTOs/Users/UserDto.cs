namespace JobPortal.Api.DTOs.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public Role Role { get; set; }
        public UserStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? AvatarUrl { get; set; }
    }

}
