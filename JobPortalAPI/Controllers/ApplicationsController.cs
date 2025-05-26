namespace JobPortalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> Apply(ApplicationCreateDto dto)
        {
            var userId = GetUserId();
            var application = await _applicationService.ApplyAsync(dto, userId);
            return Ok(application);
        }

        [HttpGet("seeker")]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> GetMyApplications()
        {
            var userId = GetUserId();
            var apps = await _applicationService.GetMyApplicationsAsync(userId);
            return Ok(apps);
        }

        [HttpGet("employer")]
        [Authorize(Roles = nameof(Role.Employer))]
        public async Task<IActionResult> GetReceivedApplications()
        {
            var employerId = GetUserId();
            var apps = await _applicationService.GetReceivedApplicationsAsync(employerId);
            return Ok(apps);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = nameof(Role.Employer))]
        public async Task<IActionResult> UpdateStatus(int id, ApplicationStatusUpdateDto dto)
        {
            var employerId = GetUserId();

            try
            {
                await _applicationService.UpdateApplicationStatusAsync(id, dto.Status, employerId);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
