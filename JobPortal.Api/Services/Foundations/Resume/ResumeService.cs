namespace JobPortal.Api.Services.Foundations.Resume
{
    public class ResumeService : IResumeService
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ResumeService(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
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

            var resume = await _db.Resumes.FirstOrDefaultAsync(r => r.SeekerId == userId);
            if (resume != null)
            {
                resume.FileUrl = $"/resumes/{safeFileName}";
                resume.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
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

            return (fileContent, contentType, Path.GetFileName(filePath));
        }

        public async Task<ResumeDto?> GetResumeByIdAsync(int id)
        {
            var resume = await _db.Resumes.FindAsync(id);
            if (resume == null) return null;

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
            var resume = await _db.Resumes.FirstOrDefaultAsync(r => r.Id == id && r.SeekerId == userId);
            if (resume == null) return false;

            _db.Resumes.Remove(resume);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateResumeAsync(int id, ResumeUpdateDto dto, int userId)
        {
            var resume = await _db.Resumes.FirstOrDefaultAsync(r => r.Id == id && r.SeekerId == userId);
            if (resume == null) return false;

            resume.FullName = dto.FullName;
            resume.Email = dto.Email;
            resume.PhoneNumber = dto.PhoneNumber;
            resume.Skills = dto.Skills;
            resume.Experience = dto.Experience;
            resume.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<(List<ResumeDto> Resumes, int TotalCount)> SearchResumesAsync(ResumeSearchDto dto)
        {
            if (dto.Page <= 0) dto.Page = 1;
            if (dto.PageSize <= 0 || dto.PageSize > 50) dto.PageSize = 10;

            var query = _db.Resumes.AsQueryable();

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

            return (resumes, totalCount);
        }
    }
}
