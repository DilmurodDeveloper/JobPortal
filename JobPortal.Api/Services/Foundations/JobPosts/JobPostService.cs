namespace JobPortal.Api.Services.Foundations.JobPosts
{
    public class JobPostService : IJobPostService
    {
        private readonly ApplicationDbContext _db;

        public JobPostService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<(List<JobPost> Jobs, int TotalCount)> GetAllJobsAsync(JobSearchDto searchDto)
        {
            if (searchDto.Page <= 0) searchDto.Page = 1;
            if (searchDto.PageSize <= 0 || searchDto.PageSize > 50) searchDto.PageSize = 10;

            var query = _db.JobPosts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchDto.Title))
                query = query.Where(j => j.Title.Contains(searchDto.Title));

            if (!string.IsNullOrWhiteSpace(searchDto.Location))
                query = query.Where(j => j.Location.Contains(searchDto.Location));

            if (searchDto.JobType.HasValue)
                query = query.Where(j => j.JobType == searchDto.JobType.Value);

            var totalCount = await query.CountAsync();

            var jobs = await query
                .OrderByDescending(j => j.CreatedAt)
                .Skip((searchDto.Page - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToListAsync();

            return (jobs, totalCount);
        }

        public async Task<JobPost?> GetJobByIdAsync(int id)
        {
            return await _db.JobPosts.FindAsync(id);
        }

        public async Task<JobPost> CreateJobAsync(JobCreateDto dto, int userId)
        {
            var jobPost = new JobPost
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

            await _db.JobPosts.AddAsync(jobPost);
            await _db.SaveChangesAsync();

            return jobPost;
        }

        public async Task<bool> UpdateJobAsync(int id, JobUpdateDto dto)
        {
            var existingJob = await _db.JobPosts.FindAsync(id);
            if (existingJob == null)
                return false;

            existingJob.Title = dto.Title!;
            existingJob.Description = dto.Description!;
            existingJob.Company = dto.Company!;
            existingJob.Location = dto.Location!;
            existingJob.Salary = dto.Salary ?? 0m;
            existingJob.JobType = dto.JobType ?? JobType.FullTime;
            existingJob.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PatchJobAsync(int id, JobUpdateDto dto)
        {
            var existingJob = await _db.JobPosts.FindAsync(id);
            if (existingJob == null)
                return false;

            if (dto.Title != null)
                existingJob.Title = dto.Title;

            if (dto.Description != null)
                existingJob.Description = dto.Description;

            if (dto.Company != null)
                existingJob.Company = dto.Company;

            if (dto.Location != null)
                existingJob.Location = dto.Location;

            if (dto.Salary != default)
                existingJob.Salary = dto.Salary.GetValueOrDefault();


            if (dto.JobType != default)
                existingJob.JobType = dto.JobType ?? JobType.FullTime;

            existingJob.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteJobAsync(int id)
        {
            var job = await _db.JobPosts.FindAsync(id);
            if (job == null)
                return false;

            _db.JobPosts.Remove(job);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApplyToJobAsync(int jobId, int seekerId)
        {
            var job = await _db.JobPosts.FindAsync(jobId);
            if (job == null) return false;

            var alreadyApplied = await _db.Applications
                .AnyAsync(a => a.JobPostId == jobId && a.UserId == seekerId);

            if (alreadyApplied) return false;

            var application = new ApplicationModel
            {
                JobPostId = jobId,
                UserId = seekerId,
                Status = ApplicationStatus.Pending,
                AppliedAt = DateTime.UtcNow
            };

            await _db.Applications.AddAsync(application);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<List<ApplicationDto>?> GetApplicantsAsync(int jobId, int employerId)
        {
            var job = await _db.JobPosts.Include(j => j.User).FirstOrDefaultAsync(j => j.Id == jobId);
            if (job == null || job.UserId != employerId)
                return null;

            var applications = await _db.Applications
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

    }
}
