namespace JobPortal.Api.Controllers
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

        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.User)}")]
        public async Task<IActionResult> GetApplicationById(int id)
        {
            try
            {
                var application = await _applicationService.GetApplicationByIdAsync(id);
                return Ok(application);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> UpdateApplication(int id, [FromBody] UpdateApplicationDto dto)
        {
            try
            {
                var userId = GetUserId();
                await _applicationService.UpdateApplicationAsync(id, dto, userId);
                return Ok(new { message = "Application updated successfully" });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.User)}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            try
            {
                var userId = GetUserId();
                var userRole = User.FindFirstValue(ClaimTypes.Role);
                await _applicationService.DeleteApplicationAsync(id, userId, userRole!);
                return Ok(new { message = "Application deleted successfully" });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> GetAllApplications(
            int page = 1,
            int pageSize = 20,
            ApplicationStatus? status = null,
            string? seekerName = null)
        {
            var result = await _applicationService.GetAllApplicationsAsync(page, pageSize, status, seekerName);
            return Ok(result);
        }
    }
}
