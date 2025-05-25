using JobPortalAPI.Data;
using JobPortalAPI.DTOs;
using JobPortalAPI.Enums;  // Enum uchun using qo‘shildi
using JobPortalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public JobController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var jobs = await _db.JobPosts.ToListAsync();
            return Ok(jobs);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var job = await _db.JobPosts.FindAsync(id);
            if (job == null) return NotFound();
            return Ok(job);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Employer) + "," + nameof(Role.Admin))]
        public async Task<IActionResult> Create(JobCreateDto dto)
        {
            var jobPost = new JobPost
            {
                Title = dto.Title,
                Description = dto.Description,
                Company = dto.Company,
                Location = dto.Location,
                Salary = dto.Salary,
                JobType = dto.JobType
            };

            await _db.JobPosts.AddAsync(jobPost);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = jobPost.Id }, jobPost);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Role.Employer) + "," + nameof(Role.Admin))]
        public async Task<IActionResult> Update(int id, JobUpdateDto jobDto)
        {
            var existingJob = await _db.JobPosts.FindAsync(id);
            if (existingJob == null) return NotFound();

            existingJob.Title = jobDto.Title;
            existingJob.Description = jobDto.Description;
            existingJob.Company = jobDto.Company;
            existingJob.Location = jobDto.Location;
            existingJob.Salary = jobDto.Salary;
            existingJob.JobType = jobDto.JobType;
            existingJob.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> Delete(int id)
        {
            var job = await _db.JobPosts.FindAsync(id);
            if (job == null) return NotFound();

            _db.JobPosts.Remove(job);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
