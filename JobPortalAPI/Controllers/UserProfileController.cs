using System.Security.Claims;
using JobPortalAPI.Data;
using JobPortalAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserProfileController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            int userId = GetUserId();
            var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null)
            {
                return NotFound("Profile not found.");
            }

            var dto = new UserProfileDto
            {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                PhoneNumber = profile.PhoneNumber,
                Address = profile.Address,
                Bio = profile.Bio,
                ProfilePictureUrl = profile.ProfilePictureUrl,
                Location = profile.Location
            };

            return Ok(dto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(UserProfileDto dto)
        {
            int userId = GetUserId();
            var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => 
                p.UserId == userId);
            
            if (profile == null)
                return NotFound("Profile not found.");
            
            profile.FirstName = dto.FirstName;
            profile.LastName = dto.LastName;
            profile.PhoneNumber = dto.PhoneNumber;
            profile.Address = dto.Address;
            profile.Bio = dto.Bio;
            profile.ProfilePictureUrl = dto.ProfilePictureUrl;
            profile.Location = dto.Location;
            profile.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProfile()
        {
            int userId = GetUserId();
            var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => 
                p.UserId == userId);
            
            if (profile == null)
                return NotFound("Profile not found.");
            
            _db.UserProfiles.Remove(profile);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        private int GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }
    }
}
