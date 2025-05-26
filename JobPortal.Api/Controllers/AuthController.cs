namespace JobPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                var token = await _authService.RegisterAsync(dto);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var token = await _authService.LoginAsync(dto);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenRequestDto tokenRequest)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(tokenRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetProfile()
        {
            var userId = _authService.GetUserIdFromClaims(User);
            if (userId == null)
                return Unauthorized();

            return Ok(new { UserId = userId });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = _authService.GetUserIdFromClaims(User);
            if (userId == null)
                return Unauthorized();

            await _authService.LogoutAsync(userId.Value);
            return Ok(new { message = "Logout successful" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            try
            {
                await _authService.ForgotPasswordAsync(dto.Email);
                return Ok(new { message = "Password reset link sent if email exists" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                await _authService.ResetPasswordAsync(dto);
                return Ok(new { message = "Password reset successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userId = _authService.GetUserIdFromClaims(User);
            if (userId == null)
                return Unauthorized();

            try
            {
                await _authService.ChangePasswordAsync(userId.Value, dto);
                return Ok(new { message = "Password changed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
