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
            var user = await _userService.GetMyUserAsync(userId);
            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMyInfo([FromBody] User userDto)
        {
            int userId = GetCurrentUserId();
            await _userService.UpdateMyUserAsync(userId, userDto);
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
        public async Task<IActionResult> PatchUser([FromBody] JsonPatchDocument<User> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            int userId = GetCurrentUserId();

            var result = await _userService.PatchUserAsync(userId, patchDoc);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty.");

            int userId = GetCurrentUserId();

            try
            {
                var avatarUrl = await _userService.UploadAvatarAsync(file, userId);
                return Ok(new { AvatarUrl = avatarUrl });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.Claims.First(c => c.Type == "id").Value);
        }
    }
}
