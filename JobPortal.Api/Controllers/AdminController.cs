using JobPortal.Api.Services.Foundations.Admin;

namespace JobPortal.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = nameof(Role.Admin))]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("users/{id}/block")]
        public async Task<IActionResult> BlockUser(int id, [FromQuery] bool block)
        {
            try
            {
                await _adminService.BlockUserAsync(id, block);
                return Ok(new { message = block ? "User blocked" : "User unblocked" });
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(
            int page = 1,
            int pageSize = 20,
            Role? role = null,
            UserStatus? status = null,
            string? search = null)
        {
            var result = await _adminService.GetUsersAsync(page, pageSize, role, status, search);
            return Ok(result);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _adminService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                await _adminService.UpdateUserAsync(id, dto);
                return Ok(new { message = "User updated successfully" });
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpPut("users/{id}/role")]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UpdateUserRoleDto dto)
        {
            try
            {
                await _adminService.UpdateUserRoleAsync(id, dto);
                return Ok(new { message = "User role updated successfully" });
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _adminService.DeleteUserAsync(id);
                return Ok(new { message = "User blocked (soft deleted)" });
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var stats = await _adminService.GetStatisticsAsync();
            return Ok(stats);
        }

        [HttpGet("applications")]
        public async Task<IActionResult> GetApplications([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var applications = await _adminService.GetApplicationsAsync(page, pageSize);
            return Ok(applications);
        }
    }
}
