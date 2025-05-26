namespace JobPortal.Api.Services.Foundations.Resume
{
    public interface IResumeService
    {
        Task<string> UploadResumeAsync(IFormFile file, int userId);
        Task<(byte[] fileContent, string contentType, string fileName)?> GetResumeAsync(int userId);
    }
}
