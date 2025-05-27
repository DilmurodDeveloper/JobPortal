namespace JobPortal.Api.Services.Foundations.Applications
{
    public partial class ApplicationService : IApplicationService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public ApplicationService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public async Task<ApplicationModel> ApplyAsync(ApplicationCreateDto dto, int userId)
        {
            try
            {
                var application = new ApplicationModel
                {
                    UserId = userId,
                    JobPostId = dto.JobPostId,
                    ResumePath = dto.ResumePath,
                    Status = ApplicationStatus.Pending,
                    AppliedAt = DateTime.UtcNow
                };

                return await this.storageBroker.InsertApplicationAsync(application);
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }

        public async Task<List<ApplicationModel>> GetMyApplicationsAsync(int userId)
        {
            try
            {
                return await this.storageBroker.SelectAllApplications()
                    .Include(a => a.JobPost)
                    .Where(a => a.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }

        public async Task<List<ApplicationModel>> GetReceivedApplicationsAsync(int employerId)
        {
            try
            {
                return await this.storageBroker.SelectAllApplications()
                    .Include(a => a.JobPost)
                    .Include(a => a.User)
                    .Where(a => a.JobPost.UserId == employerId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }

        public async Task UpdateApplicationStatusAsync(int id, ApplicationStatus status, int employerId)
        {
            try
            {
                var app = await this.storageBroker.SelectAllApplications()
                    .Include(a => a.JobPost)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (app == null)
                    throw new Exception("Application not found");

                if (app.JobPost.UserId != employerId)
                    throw new UnauthorizedAccessException("Access denied");

                app.Status = status;
                await this.storageBroker.UpdateApplicationAsync(app);
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }

        public async Task<ApplicationDto> GetApplicationByIdAsync(int id)
        {
            try
            {
                var application = await this.storageBroker.SelectAllApplications()
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
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }

        public async Task UpdateApplicationAsync(int id, UpdateApplicationDto dto, int userId)
        {
            try
            {
                var application = await this.storageBroker.SelectApplicationByIdAsync(id);

                if (application == null)
                    throw new KeyNotFoundException("Application not found");

                if (application.UserId != userId)
                    throw new UnauthorizedAccessException("You do not own this application");

                if (!string.IsNullOrEmpty(dto.ResumePath))
                    application.ResumePath = dto.ResumePath;

                await this.storageBroker.UpdateApplicationAsync(application);
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }

        public async Task DeleteApplicationAsync(int id, int userId, string userRole)
        {
            try
            {
                var application = await this.storageBroker.SelectApplicationByIdAsync(id);

                if (application == null)
                    throw new KeyNotFoundException("Application not found");

                if (userRole != nameof(Role.Admin) && application.UserId != userId)
                    throw new UnauthorizedAccessException("You do not have permission to delete this application");

                await this.storageBroker.DeleteApplicationAsync(application);
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }

        public async Task<PagedResult<ApplicationDto>> GetAllApplicationsAsync(int page, int pageSize, ApplicationStatus? status, string? seekerName)
        {
            try
            {
                var query = this.storageBroker.SelectAllApplications()
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
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }
    }
}
