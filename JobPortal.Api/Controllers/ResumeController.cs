namespace JobPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeService _resumeService;

        public ResumeController(IResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("upload")]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> UploadResume(IFormFile file)
        {
            try
            {
                var userId = GetUserId();
                var path = await _resumeService.UploadResumeAsync(file, userId);
                return Ok(new { path });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("me")]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> GetMyResume()
        {
            var userId = GetUserId();
            var resume = await _resumeService.GetResumeAsync(userId);

            if (resume == null)
                return NotFound("Resume not found.");

            return File(resume.Value.fileContent, resume.Value.contentType, resume.Value.fileName);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = nameof(Role.Admin) + "," + nameof(Role.Employer))]
        public async Task<IActionResult> GetResumeById(int id)
        {
            var resume = await _resumeService.GetResumeByIdAsync(id);
            if (resume == null) return NotFound();
            return Ok(resume);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> DeleteResume(int id)
        {
            var userId = GetUserId();
            var deleted = await _resumeService.DeleteResumeAsync(id, userId);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> UpdateResume(int id, ResumeUpdateDto dto)
        {
            var userId = GetUserId();
            var updated = await _resumeService.UpdateResumeAsync(id, dto, userId);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpGet("search")]
        [Authorize(Roles = nameof(Role.Admin) + "," + nameof(Role.Employer))]
        public async Task<IActionResult> SearchResumes([FromQuery] ResumeSearchDto searchDto)
        {
            var (resumes, totalCount) = await _resumeService.SearchResumesAsync(searchDto);
            return Ok(new
            {
                TotalCount = totalCount,
                Page = searchDto.Page,
                PageSize = searchDto.PageSize,
                Data = resumes
            });
        }
    }
}
