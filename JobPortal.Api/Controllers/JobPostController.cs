namespace JobPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobPostController : ControllerBase
    {
        private readonly IJobPostService _jobPostService;

        public JobPostController(IJobPostService jobService)
        {
            _jobPostService = jobService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] JobSearchDto searchDto)
        {
            var (jobPosts, totalCount) = await _jobPostService.GetAllJobsAsync(searchDto);

            return Ok(new
            {
                TotalCount = totalCount,
                Page = searchDto.Page,
                PageSize = searchDto.PageSize,
                Data = jobPosts
            });
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var job = await _jobPostService.GetJobByIdAsync(id);
            if (job == null) return NotFound();
            return Ok(job);
        }

        [HttpGet("{id}/applicants")]
        [Authorize(Roles = nameof(Role.Employer) + "," + nameof(Role.Admin))]
        public async Task<IActionResult> GetApplicants(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var applicants = await _jobPostService.GetApplicantsAsync(id, userId);

            if (applicants == null) return NotFound(new { message = "Job not found or access denied." });

            return Ok(applicants);
        }


        [HttpPost]
        [Authorize(Roles = nameof(Role.Employer) + "," + nameof(Role.Admin))]
        public async Task<IActionResult> Create(JobCreateDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var job = await _jobPostService.CreateJobAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = job.Id }, job);
        }

        [HttpPost("{id}/apply")]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> ApplyToJob(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _jobPostService.ApplyToJobAsync(id, userId);
            if (!result) return BadRequest(new { message = "You may have already applied or job not found." });

            return Ok(new { message = "Application submitted successfully" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Role.Employer) + "," + nameof(Role.Admin))]
        public async Task<IActionResult> Update(int id, JobUpdateDto dto)
        {
            var result = await _jobPostService.UpdateJobAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = nameof(Role.Employer) + "," + nameof(Role.Admin))]
        public async Task<IActionResult> Patch(int id, JobUpdateDto dto)
        {
            var result = await _jobPostService.PatchJobAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _jobPostService.DeleteJobAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
