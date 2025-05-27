namespace JobPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyInfo()
        {
            int userId = GetCurrentUserId();
            var userDto = await _userService.GetMyUserAsync(userId);
            return Ok(userDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMyInfo([FromBody] UserSelfUpdateDto updateDto)
        {
            if (updateDto == null)
                return BadRequest("Invalid data");

            int userId = GetCurrentUserId();
            await _userService.UpdateMyUserAsync(userId, updateDto);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMyAccount()
        {
            int userId = GetCurrentUserId();
            await _userService.DeleteMyAccountAsync(userId);
            return NoContent();
        }

        [HttpPatch]
        public async Task<IActionResult> PatchUser([FromBody] JsonPatchDocument<UserSelfPatchDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Patch data is required.");

            int userId = GetCurrentUserId();

            var result = await _userService.PatchUserAsync(userId, patchDoc);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAvatar([FromForm] UploadAvatarDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("File is empty.");

            int userId = GetCurrentUserId();

            try
            {
                var avatarUrl = await _userService.UploadAvatarAsync(dto.File, userId);
                return Ok(new { AvatarUrl = avatarUrl });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private int GetCurrentUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new UnauthorizedAccessException("User ID not found in token.");

            return int.Parse(userId);
        }
    }
}
