namespace JobPortalAPI.Services.Application
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
    }
}
