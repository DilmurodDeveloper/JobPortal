using JobPortalAPI.DTOs.Application;
using JobPortalAPI.Enums;
using ApplicationModel = JobPortalAPI.Models.Application;

namespace JobPortalAPI.Services.Application 
{
    public interface IApplicationService
    {
        Task<ApplicationModel> ApplyAsync(ApplicationCreateDto dto, int userId);
        Task<List<ApplicationModel>> GetMyApplicationsAsync(int userId);
        Task<List<ApplicationModel>> GetReceivedApplicationsAsync(int employerId);
        Task UpdateApplicationStatusAsync(int id, ApplicationStatus status, int employerId);
    }
}
