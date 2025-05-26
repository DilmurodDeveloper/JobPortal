namespace JobPortalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] JobSearchDto searchDto)
        {
            var (jobs, totalCount) = await _jobService.GetAllJobsAsync(searchDto);

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
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null) return NotFound();
            return Ok(job);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Employer) + "," + nameof(Role.Admin))]
        public async Task<IActionResult> Create(JobCreateDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var job = await _jobService.CreateJobAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = job.Id }, job);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Role.Employer) + "," + nameof(Role.Admin))]
        public async Task<IActionResult> Update(int id, JobUpdateDto dto)
        {
            var result = await _jobService.UpdateJobAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _jobService.DeleteJobAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
