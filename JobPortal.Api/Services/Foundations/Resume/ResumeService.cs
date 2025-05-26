namespace JobPortal.Api.Services.Foundations.Resume
{
    public class ResumeService : IResumeService
    {
        private readonly IWebHostEnvironment _env;

        public ResumeService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadResumeAsync(IFormFile file, int userId)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var extension = Path.GetExtension(file.FileName);
            var allowedExtensions = new[] { ".pdf", ".docx" };
            if (!allowedExtensions.Contains(extension.ToLower()))
                throw new ArgumentException("Unsupported file type.");

            var fileName = $"resume_{userId}{extension}";
            var savePath = Path.Combine(_env.WebRootPath, "resumes");

            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            var filePath = Path.Combine(savePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/resumes/{fileName}";
        }

        public async Task<(byte[] fileContent, string contentType, string fileName)?> GetResumeAsync(int userId)
        {
            var resumeFolder = Path.Combine(_env.WebRootPath, "resumes");
            if (!Directory.Exists(resumeFolder))
                return null;

            var files = Directory.GetFiles(resumeFolder, $"resume_{userId}.*");
            var filePath = files.FirstOrDefault();

            if (filePath == null)
                return null;

            var fileContent = await File.ReadAllBytesAsync(filePath);
            var extension = Path.GetExtension(filePath).ToLower();

            var contentType = extension switch
            {
                ".pdf" => "application/pdf",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };

            var fileName = Path.GetFileName(filePath);

            return (fileContent, contentType, fileName);
        }
    }
}
