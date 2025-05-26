using JobPortal.Api.Services.Foundations.Users;

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

        private int GetCurrentUserId()
        {
            return int.Parse(User.Claims.First(c => c.Type == "id").Value);
        }
    }
}
