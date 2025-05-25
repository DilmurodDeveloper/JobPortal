using System.Security.Claims;
using JobPortalAPI.Data;
using JobPortalAPI.DTOs.Job;
using JobPortalAPI.Enums;
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
        public async Task<IActionResult> GetAll([FromQuery] JobSearchDto searchDto)
        {
            if (searchDto.Page <= 0) searchDto.Page = 1;
            if (searchDto.PageSize <= 0 || searchDto.PageSize > 50) searchDto.PageSize = 10;

            var query = _db.JobPosts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchDto.Title))
                query = query.Where(j => j.Title.Contains(searchDto.Title));

            if (!string.IsNullOrWhiteSpace(searchDto.Location))
                query = query.Where(j => j.Location.Contains(searchDto.Location));

            if (searchDto.JobType.HasValue)
                query = query.Where(j => j.JobType == searchDto.JobType.Value);

            var totalCount = await query.CountAsync();

            var jobs = await query
                .OrderByDescending(j => j.CreatedAt)
                .Skip((searchDto.Page - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToListAsync();

            return Ok(new
            {
                TotalCount = totalCount,
                Page = searchDto.Page,
                PageSize = searchDto.PageSize,
                Data = jobs
            });
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
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var jobPost = new JobPost
            {
                Title = dto.Title,
                Description = dto.Description,
                Company = dto.Company,
                Location = dto.Location,
                Salary = dto.Salary,
                JobType = dto.JobType,
                UserId = userId
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
