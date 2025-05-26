namespace JobPortal.Api.Services.Foundations.UserProfiles
{
    public interface IUserProfileService
    {
        Task<UserProfileDto?> GetProfileAsync(int userId);
        Task<bool> UpdateProfileAsync(int userId, UserProfileDto dto);
        Task<bool> DeleteProfileAsync(int userId);
    }
}
