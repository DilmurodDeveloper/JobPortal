namespace JobPortal.Api.Services.Application 
{
    public interface IApplicationService
    {
        Task<ApplicationModel> ApplyAsync(ApplicationCreateDto dto, int userId);
        Task<List<ApplicationModel>> GetMyApplicationsAsync(int userId);
        Task<List<ApplicationModel>> GetReceivedApplicationsAsync(int employerId);
        Task UpdateApplicationStatusAsync(int id, ApplicationStatus status, int employerId);
    }
}
