using JobPortalAPI.Enums;

namespace JobPortalAPI.DTOs.Auth
{
    public class RegisterDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Role? Role { get; set; }
    }
}
