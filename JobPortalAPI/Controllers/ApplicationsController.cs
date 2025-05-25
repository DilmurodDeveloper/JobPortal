using JobPortalAPI.Data;
using JobPortalAPI.Enums;
using JobPortalAPI.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using JobPortalAPI.DTOs.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace JobPortalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ApplicationsController(ApplicationDbContext db)
        {
            _db = db;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> Apply(ApplicationCreateDto dto)
        {
            var application = new Application
            {
                UserId = GetUserId(),
                JobPostId = dto.JobPostId,
                ResumePath = dto.ResumePath,
                Status = ApplicationStatus.Pending,
                AppliedAt = DateTime.UtcNow
            };

            await _db.Applications.AddAsync(application);
            await _db.SaveChangesAsync();

            return Ok(application);
        }

        [HttpGet("seeker")]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> GetMyApplications()
        {
            int userId = GetUserId();

            var apps = await _db.Applications
                .Include(a => a.JobPost)
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return Ok(apps);
        }

        [HttpGet("employer")]
        [Authorize(Roles = nameof(Role.Employer))]
        public async Task<IActionResult> GetReceivedApplications()
        {
            var employerId = GetUserId();

            var apps = await _db.Applications
                .Include(a => a.JobPost)
                .Include(a => a.User)
                .Where(a => a.JobPost.UserId == employerId)
                .ToListAsync();

            return Ok(apps);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = nameof(Role.Employer))]
        public async Task<IActionResult> UpdateStatus(int id, ApplicationStatusUpdateDto dto)
        {
            var app = await _db.Applications
                .Include(a => a.JobPost)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (app == null)
                return NotFound();

            if (app.JobPost.UserId != GetUserId())
                return Forbid();

            app.Status = dto.Status;
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
