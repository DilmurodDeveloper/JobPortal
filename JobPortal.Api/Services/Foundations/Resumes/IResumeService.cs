namespace JobPortal.Api.Services.Foundations.Resumes
{
    public interface IResumeService
    {
        Task<string> UploadResumeAsync(IFormFile file, int userId);
        Task<(byte[] fileContent, string contentType, string fileName)?> GetResumeAsync(int userId);
        Task<ResumeDto?> GetResumeByIdAsync(int id);
        Task<bool> DeleteResumeAsync(int id, int userId);
        Task<bool> UpdateResumeAsync(int id, ResumeUpdateDto dto, int userId);
        Task<(List<ResumeDto> Resumes, int TotalCount)> SearchResumesAsync(ResumeSearchDto dto);
    }
}
