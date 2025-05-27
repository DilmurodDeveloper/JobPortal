namespace JobPortal.Api.DTOs.Admins
{
    public class AdminUpdateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public Role? Role { get; set; }
    }
}
