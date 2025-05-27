namespace JobPortal.Api.Services.Foundations.Admin
{
    public interface IAdminService
    {
        Task BlockUserAsync(int id, bool block);

        Task<PagedResult<UserSummaryDto>> GetUsersAsync(
            int page,
            int pageSize,
            Role? role,
            UserStatus? status,
            string? search);

        Task<UserDetailsDto> GetUserByIdAsync(int id);

        Task<PagedResult<ApplicationDto>> GetApplicationsAsync(int page, int pageSize);

        Task UpdateUserAsync(int id, AdminUpdateUserDto dto);
        Task DeleteUserAsync(int id);
        Task<StatisticsDto> GetStatisticsAsync();
        Task UpdateUserRoleAsync(int id, UpdateUserRoleDto dto);
    }
}
