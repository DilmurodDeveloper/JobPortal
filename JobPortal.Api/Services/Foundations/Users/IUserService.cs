namespace JobPortal.Api.Services.Foundations.Users
{
    public interface IUserService
    {
        Task<UserSelfViewDto> GetMyUserAsync(int userId);
        Task UpdateMyUserAsync(int userId, UserSelfUpdateDto updateDto);
        Task DeleteMyAccountAsync(int userId);
        Task<bool> PatchUserAsync(int userId, JsonPatchDocument<UserSelfPatchDto> patchDoc);
        Task<string> UploadAvatarAsync(IFormFile file, int userId);
    }
}
