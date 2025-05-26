namespace JobPortal.Api.Services.UserProfile
{
    public interface IUserProfileService
    {
        Task<UserProfileDto?> GetProfileAsync(int userId);
        Task<bool> UpdateProfileAsync(int userId, UserProfileDto dto);
        Task<bool> DeleteProfileAsync(int userId);
    }
}
