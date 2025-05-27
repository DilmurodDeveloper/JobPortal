namespace JobPortal.Api.Services.Foundations.Resumes
{
    public partial class ResumeService : IResumeService
    {
        private readonly IStorageBroker _storageBroker;
        private readonly ILoggingBroker _loggingBroker;
        private readonly IWebHostEnvironment _env;

        public ResumeService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IWebHostEnvironment env)
        {
            _storageBroker = storageBroker;
            _loggingBroker = loggingBroker;
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

            var safeFileName = $"resume_{userId}{extension}";
            var resumeFolder = Path.Combine(_env.WebRootPath, "resumes");

            if (!Directory.Exists(resumeFolder))
                Directory.CreateDirectory(resumeFolder);

            var filePath = Path.Combine(resumeFolder, safeFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var resume = await _storageBroker.SelectAllResumes()
                            .FirstOrDefaultAsync(r => r.SeekerId == userId);

            if (resume != null)
            {
                resume.FileUrl = $"/resumes/{safeFileName}";
                resume.UpdatedAt = DateTime.UtcNow;

                await _storageBroker.UpdateResumeAsync(resume);

                _loggingBroker.LogInformation($"Resume updated for userId: {userId}");
            }
            else
            {
                var newResume = new Resume
                {
                    SeekerId = userId,
                    FileUrl = $"/resumes/{safeFileName}",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    FullName = string.Empty,
                    Email = string.Empty,
                    PhoneNumber = string.Empty,
                    Skills = string.Empty,
                    Experience = string.Empty,
                    Location = null
                };


                await _storageBroker.InsertResumeAsync(newResume);

                _loggingBroker.LogInformation($"Resume uploaded for new userId: {userId}");
            }

            return $"/resumes/{safeFileName}";
        }

        public async Task<(byte[] fileContent, string contentType, string fileName)?> GetResumeAsync(int userId)
        {
            var resumeFolder = Path.Combine(_env.WebRootPath, "resumes");

            if (!Directory.Exists(resumeFolder))
                return null;

            var possibleFiles = Directory.GetFiles(resumeFolder, $"resume_{userId}.*");
            var filePath = possibleFiles.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return null;

            byte[] fileContent = await File.ReadAllBytesAsync(filePath);

            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            var contentType = extension switch
            {
                ".pdf" => "application/pdf",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };

            _loggingBroker.LogInformation($"Resume retrieved for userId: {userId}");

            return (fileContent, contentType, Path.GetFileName(filePath));
        }

        public async Task<ResumeDto?> GetResumeByIdAsync(int id)
        {
            var resume = await _storageBroker.SelectResumeByIdAsync(id);
            if (resume == null)
            {
                _loggingBroker.LogWarning($"Resume with id {id} not found.");
                return null;
            }

            return new ResumeDto
            {
                Id = resume.Id,
                SeekerId = resume.SeekerId,
                FullName = resume.FullName,
                Email = resume.Email,
                PhoneNumber = resume.PhoneNumber,
                Skills = resume.Skills,
                Experience = resume.Experience
            };
        }

        public async Task<bool> DeleteResumeAsync(int id, int userId)
        {
            var resume = await _storageBroker.SelectAllResumes()
                            .FirstOrDefaultAsync(r => r.Id == id && r.SeekerId == userId);

            if (resume == null)
            {
                _loggingBroker.LogWarning($"Attempted to delete non-existing resume id: {id} for userId: {userId}");
                return false;
            }

            await _storageBroker.DeleteResumeAsync(resume);
            _loggingBroker.LogInformation($"Deleted resume id: {id} for userId: {userId}");
            return true;
        }

        public async Task<bool> UpdateResumeAsync(int id, ResumeUpdateDto dto, int userId)
        {
            var resume = await _storageBroker.SelectAllResumes()
                            .FirstOrDefaultAsync(r => r.Id == id && r.SeekerId == userId);

            if (resume == null)
            {
                _loggingBroker.LogWarning($"Attempted to update non-existing resume id: {id} for userId: {userId}");
                return false;
            }

            resume.FullName = dto.FullName;
            resume.Email = dto.Email;
            resume.PhoneNumber = dto.PhoneNumber;
            resume.Skills = dto.Skills;
            resume.Experience = dto.Experience;
            resume.UpdatedAt = DateTime.UtcNow;

            await _storageBroker.UpdateResumeAsync(resume);

            _loggingBroker.LogInformation($"Updated resume id: {id} for userId: {userId}");

            return true;
        }

        public async Task<(List<ResumeDto> Resumes, int TotalCount)> SearchResumesAsync(ResumeSearchDto dto)
        {
            if (dto.Page <= 0) dto.Page = 1;
            if (dto.PageSize <= 0 || dto.PageSize > 50) dto.PageSize = 10;

            var query = _storageBroker.SelectAllResumes();

            if (!string.IsNullOrWhiteSpace(dto.Skill))
            {
                query = query.Where(r => r.Skills.Contains(dto.Skill));
            }

            if (!string.IsNullOrWhiteSpace(dto.Location))
            {
                query = query.Where(r => r.Location != null && r.Location.Contains(dto.Location));
            }

            var totalCount = await query.CountAsync();

            var resumes = await query
                .OrderByDescending(r => r.UpdatedAt)
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .Select(r => new ResumeDto
                {
                    Id = r.Id,
                    SeekerId = r.SeekerId,
                    FullName = r.FullName,
                    Email = r.Email,
                    PhoneNumber = r.PhoneNumber,
                    Skills = r.Skills,
                    Experience = r.Experience
                })
                .ToListAsync();

            _loggingBroker.LogInformation($"Search completed with {resumes.Count} results out of {totalCount}");

            return (resumes, totalCount);
        }
    }
}
