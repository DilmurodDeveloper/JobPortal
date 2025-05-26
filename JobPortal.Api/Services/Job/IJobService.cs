using JobPortal.Api.Models.Foundations.JobPosts;

namespace JobPortal.Api.Services.Job
{
    public interface IJobService
    {
        Task<(List<JobPost> Jobs, int TotalCount)> GetAllJobsAsync(JobSearchDto searchDto);
        Task<JobPost?> GetJobByIdAsync(int id);
        Task<JobPost> CreateJobAsync(JobCreateDto dto, int userId);
        Task<bool> UpdateJobAsync(int id, JobUpdateDto dto);
        Task<bool> DeleteJobAsync(int id);
    }
}
