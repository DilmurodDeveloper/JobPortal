﻿namespace JobPortal.Api.DTOs.Admins
{
    public class UserSummaryDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
