namespace JobPortal.Api.Services.Foundations.Applications
{
    public interface IApplicationService
    {
        Task<ApplicationModel> ApplyAsync(ApplicationCreateDto dto, int userId);
        Task<List<ApplicationModel>> GetMyApplicationsAsync(int userId);
        Task<List<ApplicationModel>> GetReceivedApplicationsAsync(int employerId);
        Task UpdateApplicationStatusAsync(int id, ApplicationStatus status, int employerId);
        Task<ApplicationDto> GetApplicationByIdAsync(int id);
        Task UpdateApplicationAsync(int id, UpdateApplicationDto dto, int userId);
        Task DeleteApplicationAsync(int id, int userId, string userRole);
        Task<PagedResult<ApplicationDto>> GetAllApplicationsAsync(
            int page, int pageSize, ApplicationStatus? status, string? seekerName);

    }
}
