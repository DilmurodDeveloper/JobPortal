using JobPortalAPI.Data;
using JobPortalAPI.DTOs.Admin;
using JobPortalAPI.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = nameof(Role.Admin))]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("users/{id}/block")]
        public async Task<IActionResult> BlockUser(int id, [FromQuery] bool block)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound(new { message = "User not found" });

            user.Status = block ? UserStatus.Blocked : UserStatus.Active;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = block ? "User blocked" : "User unblocked" });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(
            int page = 1,
            int pageSize = 20,
            Role? role = null,
            UserStatus? status = null,
            string? search = null)
        {
            var query = _context.Users.AsQueryable();

            if (role.HasValue)
                query = query.Where(u => u.Role == role.Value);

            if (status.HasValue)
                query = query.Where(u => u.Status == status.Value);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(u => u.FirstName!.Contains(search)
                                      || u.LastName!.Contains(search)
                                      || u.Email!.Contains(search));

            var totalUsers = await query.CountAsync();

            var users = await query
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    Role = u.Role.ToString(),
                    Status = u.Status.ToString(),
                    u.CreatedAt
                })
                .ToListAsync();

            return Ok(new
            {
                Total = totalUsers,
                Page = page,
                PageSize = pageSize,
                Users = users
            });
        }  

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound(new { message = "User not found" });

            user.FirstName = dto.FirstName ?? user.FirstName;
            user.LastName = dto.LastName ?? user.LastName;
            user.Email = dto.Email ?? user.Email;

            if (dto.Role.HasValue)
                user.Role = dto.Role.Value;

            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "User updated successfully" });
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound(new { message = "User not found" });

            user.Status = UserStatus.Blocked;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "User blocked (soft deleted)" });
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var totalUsers = await _context.Users.CountAsync();
            var blockedUsers = await _context.Users.CountAsync(u => u.Status == UserStatus.Blocked);
            var admins = await _context.Users.CountAsync(u => u.Role == Role.Admin);
            var employers = await _context.Users.CountAsync(u => u.Role == Role.Employer);
            var regularUsers = await _context.Users.CountAsync(u => u.Role == Role.User);

            return Ok(new
            {
                TotalUsers = totalUsers,
                BlockedUsers = blockedUsers,
                AdminCount = admins,
                EmployerCount = employers,
                UserCount = regularUsers
            });
        }

        [HttpPut("users/{id}/role")]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UpdateUserRoleDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound(new { message = "User not found" });

            if (!Enum.IsDefined(typeof(Role), dto.Role))
                return BadRequest(new { message = "Invalid role value" });

            user.Role = dto.Role;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "User role updated successfully" });
        }
    }
}
