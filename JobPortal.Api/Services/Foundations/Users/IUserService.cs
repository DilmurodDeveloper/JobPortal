namespace JobPortal.Api.Services.Foundations.Users
{
    public interface IUserService
    {
        Task<User> GetMyUserAsync(int userId);
        Task UpdateMyUserAsync(int userId, User updatedUser);
        Task DeleteMyAccountAsync(int userId);
    }
}
