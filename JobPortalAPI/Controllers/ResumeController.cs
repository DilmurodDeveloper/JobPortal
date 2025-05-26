namespace JobPortalAPI.Controllers
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

        [HttpGet]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> GetMyResume()
        {
            var userId = GetUserId();
            var resume = await _resumeService.GetResumeAsync(userId);

            if (resume == null)
                return NotFound("Resume not found.");

            return File(resume.Value.fileContent, resume.Value.contentType, resume.Value.fileName);
        }
    }
}
