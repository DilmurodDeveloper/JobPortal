namespace JobPortal.Api.Services.Foundations.Application
{
    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationDbContext _db;

        public ApplicationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ApplicationModel> ApplyAsync(ApplicationCreateDto dto, int userId)
        {
            var application = new ApplicationModel
            {
                UserId = userId,
                JobPostId = dto.JobPostId,
                ResumePath = dto.ResumePath,
                Status = ApplicationStatus.Pending,
                AppliedAt = DateTime.UtcNow
            };

            await _db.Applications.AddAsync(application);
            await _db.SaveChangesAsync();

            return application;
        }

        public async Task<List<ApplicationModel>> GetMyApplicationsAsync(int userId)
        {
            return await _db.Applications
                .Include(a => a.JobPost)
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<ApplicationModel>> GetReceivedApplicationsAsync(int employerId)
        {
            return await _db.Applications
                .Include(a => a.JobPost)
                .Include(a => a.User)
                .Where(a => a.JobPost.UserId == employerId)
                .ToListAsync();
        }

        public async Task UpdateApplicationStatusAsync(int id, ApplicationStatus status, int employerId)
        {
            var app = await _db.Applications
                .Include(a => a.JobPost)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (app == null)
                throw new Exception("Application not found");

            if (app.JobPost.UserId != employerId)
                throw new UnauthorizedAccessException("Access denied");

            app.Status = status;
            await _db.SaveChangesAsync();
        }

        public async Task<ApplicationDto> GetApplicationByIdAsync(int id)
        {
            var application = await _db.Applications
                .Include(a => a.User)
                .Include(a => a.JobPost)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
                throw new KeyNotFoundException("Application not found");

            return new ApplicationDto
            {
                Id = application.Id,
                JobId = application.JobPostId,
                SeekerId = application.UserId,
                Status = application.Status.ToString(),
                CreatedAt = application.AppliedAt,
                JobTitle = application.JobPost.Title,
                SeekerName = application.User.FirstName + " " + application.User.LastName
            };
        }

        public async Task UpdateApplicationAsync(int id, UpdateApplicationDto dto, int userId)
        {
            var application = await _db.Applications.FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
                throw new KeyNotFoundException("Application not found");

            if (application.UserId != userId)
                throw new UnauthorizedAccessException("You do not own this application");

            if (!string.IsNullOrEmpty(dto.ResumePath))
                application.ResumePath = dto.ResumePath;

            await _db.SaveChangesAsync();
        }

        public async Task DeleteApplicationAsync(int id, int userId, string userRole)
        {
            var application = await _db.Applications.FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
                throw new KeyNotFoundException("Application not found");

            if (userRole != nameof(Role.Admin) && application.UserId != userId)
                throw new UnauthorizedAccessException("You do not have permission to delete this application");

            _db.Applications.Remove(application);
            await _db.SaveChangesAsync();
        }

        public async Task<PagedResult<ApplicationDto>> GetAllApplicationsAsync(int page, int pageSize, ApplicationStatus? status, string? seekerName)
        {
            var query = _db.Applications
                .Include(a => a.User)
                .Include(a => a.JobPost)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(a => a.Status == status.Value);

            if (!string.IsNullOrWhiteSpace(seekerName))
                query = query.Where(a => (a.User.FirstName + " " + a.User.LastName).Contains(seekerName));

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(a => a.AppliedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new ApplicationDto
                {
                    Id = a.Id,
                    JobId = a.JobPostId,
                    SeekerId = a.UserId,
                    Status = a.Status.ToString(),
                    CreatedAt = a.AppliedAt,
                    JobTitle = a.JobPost.Title,
                    SeekerName = a.User.FirstName + " " + a.User.LastName
                })
                .ToListAsync();

            return new PagedResult<ApplicationDto>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Items = items
            };
        }
    }
}
