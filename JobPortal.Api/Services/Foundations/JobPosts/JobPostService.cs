namespace JobPortal.Api.Services.Foundations.JobPosts
{
    public partial class JobPostService : IJobPostService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public JobPostService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public async Task<(List<JobPost> Jobs, int TotalCount)> GetAllJobsAsync(JobSearchDto searchDto)
        {
            try
            {
                if (searchDto.Page <= 0) searchDto.Page = 1;
                if (searchDto.PageSize <= 0 || searchDto.PageSize > 50) searchDto.PageSize = 10;

                IQueryable<JobPost> query = this.storageBroker.SelectAllJobPosts();

                if (!string.IsNullOrWhiteSpace(searchDto.Title))
                    query = query.Where(j => j.Title.Contains(searchDto.Title));

                if (!string.IsNullOrWhiteSpace(searchDto.Location))
                    query = query.Where(j => j.Location.Contains(searchDto.Location));

                if (searchDto.JobType.HasValue)
                    query = query.Where(j => j.JobType == searchDto.JobType.Value);

                int totalCount = await query.CountAsync();

                List<JobPost> jobs = await query
                    .OrderByDescending(j => j.CreatedAt)
                    .Skip((searchDto.Page - 1) * searchDto.PageSize)
                    .Take(searchDto.PageSize)
                    .ToListAsync();

                return (jobs, totalCount);
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }

        public async Task<JobPost?> GetJobByIdAsync(int id)
        {
            try
            {
                return await this.storageBroker.SelectJobPostByIdAsync(id);
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }

        public async Task<JobPost> CreateJobAsync(JobCreateDto dto, int userId)
        {
            try
            {
                JobPost jobPost = new JobPost
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Company = dto.Company,
                    Location = dto.Location,
                    Salary = dto.Salary,
                    JobType = dto.JobType,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                return await this.storageBroker.InsertJobPostAsync(jobPost);
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }

        public async Task<bool> UpdateJobAsync(int id, JobUpdateDto dto)
        {
            try
            {
                JobPost? existingJob = await this.storageBroker.SelectJobPostByIdAsync(id);
                if (existingJob == null)
                    return false;

                existingJob.Title = dto.Title ?? existingJob.Title;
                existingJob.Description = dto.Description ?? existingJob.Description;
                existingJob.Company = dto.Company ?? existingJob.Company;
                existingJob.Location = dto.Location ?? existingJob.Location;
                existingJob.Salary = dto.Salary ?? existingJob.Salary;
                existingJob.JobType = dto.JobType ?? existingJob.JobType;
                existingJob.UpdatedAt = DateTime.UtcNow;

                await this.storageBroker.UpdateJobPostAsync(existingJob);
                return true;
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }

        public async Task<bool> PatchJobAsync(int id, JobUpdateDto dto)
        {
            try
            {
                JobPost? existingJob = await this.storageBroker.SelectJobPostByIdAsync(id);
                if (existingJob == null)
                    return false;

                if (!string.IsNullOrWhiteSpace(dto.Title))
                    existingJob.Title = dto.Title;

                if (!string.IsNullOrWhiteSpace(dto.Description))
                    existingJob.Description = dto.Description;

                if (!string.IsNullOrWhiteSpace(dto.Company))
                    existingJob.Company = dto.Company;

                if (!string.IsNullOrWhiteSpace(dto.Location))
                    existingJob.Location = dto.Location;

                if (dto.Salary.HasValue)
                    existingJob.Salary = dto.Salary.Value;

                if (dto.JobType.HasValue)
                    existingJob.JobType = dto.JobType.Value;

                existingJob.UpdatedAt = DateTime.UtcNow;

                await this.storageBroker.UpdateJobPostAsync(existingJob);
                return true;
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }

        public async Task<bool> DeleteJobAsync(int id)
        {
            try
            {
                JobPost? job = await this.storageBroker.SelectJobPostByIdAsync(id);
                if (job == null)
                    return false;

                await this.storageBroker.DeleteJobPostAsync(job);
                return true;
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }

        public async Task<bool> ApplyToJobAsync(int jobId, int seekerId)
        {
            try
            {
                JobPost? job = await this.storageBroker.SelectJobPostByIdAsync(jobId);
                if (job == null)
                    return false;

                bool alreadyApplied = await this.storageBroker
                    .SelectAllApplications()
                    .AnyAsync(a => a.JobPostId == jobId && a.UserId == seekerId);

                if (alreadyApplied)
                    return false;

                var application = new ApplicationModel
                {
                    JobPostId = jobId,
                    UserId = seekerId,
                    Status = ApplicationStatus.Pending,
                    AppliedAt = DateTime.UtcNow
                };

                await this.storageBroker.InsertApplicationAsync(application);
                return true;
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }

        public async Task<List<ApplicationDto>?> GetApplicantsAsync(int jobId, int employerId)
        {
            try
            {
                JobPost? job = await this.storageBroker.SelectJobPostByIdAsync(jobId);
                if (job == null || job.UserId != employerId)
                    return null;

                var applications = await this.storageBroker.SelectAllApplications()
                    .Where(a => a.JobPostId == jobId)
                    .Include(a => a.User)
                    .Select(a => new ApplicationDto
                    {
                        Id = a.Id,
                        JobId = a.JobPostId,
                        SeekerId = a.UserId,
                        Status = a.Status.ToString(),
                        CreatedAt = a.AppliedAt,
                        JobTitle = job.Title,
                        SeekerName = a.User.FirstName + " " + a.User.LastName
                    })
                    .ToListAsync();

                return applications;
            }
            catch (Exception ex)
            {
                this.loggingBroker.LogError(ex);
                throw;
            }
        }
    }
}
