namespace JobPortal.Api.Services.Admin
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

        Task UpdateUserAsync(int id, UpdateUserDto dto);
        Task DeleteUserAsync(int id);
        Task<StatisticsDto> GetStatisticsAsync();
        Task UpdateUserRoleAsync(int id, UpdateUserRoleDto dto);
    }
}
