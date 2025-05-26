namespace JobPortalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetUserId();
            var profile = await _userProfileService.GetProfileAsync(userId);
            if (profile == null)
                return NotFound("Profile not found.");
            return Ok(profile);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(UserProfileDto dto)
        {
            var userId = GetUserId();
            var updated = await _userProfileService.UpdateProfileAsync(userId, dto);
            if (!updated)
                return NotFound("Profile not found.");
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProfile()
        {
            var userId = GetUserId();
            var deleted = await _userProfileService.DeleteProfileAsync(userId);
            if (!deleted)
                return NotFound("Profile not found.");
            return NoContent();
        }
    }
}
