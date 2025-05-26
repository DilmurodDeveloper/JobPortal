using JobPortalAPI.Data;
using JobPortalAPI.DTOs.User;
using Microsoft.EntityFrameworkCore;

namespace JobPortalAPI.Services.UserProfile
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ApplicationDbContext _db;

        public UserProfileService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<UserProfileDto?> GetProfileAsync(int userId)
        {
            var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return null;

            return new UserProfileDto
            {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                PhoneNumber = profile.PhoneNumber,
                Address = profile.Address,
                Bio = profile.Bio,
                ProfilePictureUrl = profile.ProfilePictureUrl,
                Location = profile.Location
            };
        }

        public async Task<bool> UpdateProfileAsync(int userId, UserProfileDto dto)
        {
            var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return false;

            profile.FirstName = dto.FirstName;
            profile.LastName = dto.LastName;
            profile.PhoneNumber = dto.PhoneNumber;
            profile.Address = dto.Address;
            profile.Bio = dto.Bio;
            profile.ProfilePictureUrl = dto.ProfilePictureUrl;
            profile.Location = dto.Location;
            profile.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProfileAsync(int userId)
        {
            var profile = await _db.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return false;

            _db.UserProfiles.Remove(profile);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
