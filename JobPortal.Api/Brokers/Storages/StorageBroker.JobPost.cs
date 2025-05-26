using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JobPortal.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<JobPost> JobPosts { get; set; } = null!;

        public async ValueTask<JobPost> InsertJobPostAsync(JobPost jobPost)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<JobPost> jobPostEntityEntry = await broker.JobPosts.AddAsync(jobPost);

            await broker.SaveChangesAsync();

            return jobPostEntityEntry.Entity;
        }

        public IQueryable<JobPost> SelectAllJobPosts() =>
            SelectAll<JobPost>();

        public async ValueTask<JobPost> SelectJobPostByIdAsync(int jobPostId) =>
            await SelectAsync<JobPost>(jobPostId);

        public async ValueTask<JobPost> UpdateJobPostAsync(JobPost jobPost) =>
            await UpdateAsync(jobPost);

        public async ValueTask<JobPost> DeleteJobPostAsync(JobPost jobPost) =>
            await DeleteAsync<JobPost>(jobPost);
    }
}
