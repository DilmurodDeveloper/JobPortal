using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JobPortal.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<JobPost> JobPosts { get; set; } = null!;

        public async ValueTask<JobPost> InsertJobPostAsync(JobPost jobPost)
        {
            EntityEntry<JobPost> jobPostEntityEntry = await this.JobPosts.AddAsync(jobPost);
            await this.SaveChangesAsync();
            return jobPostEntityEntry.Entity;
        }

        public IQueryable<JobPost> SelectAllJobPosts() =>
            SelectAll<JobPost>();

        public async ValueTask<JobPost?> SelectJobPostByIdAsync(int jobPostId) =>
            await SelectAsync<JobPost>(jobPostId);

        public async ValueTask<JobPost> UpdateJobPostAsync(JobPost jobPost)
        {
            EntityEntry<JobPost> jobPostEntityEntry = this.JobPosts.Update(jobPost);
            await this.SaveChangesAsync();
            return jobPostEntityEntry.Entity;
        }

        public async ValueTask<JobPost> DeleteJobPostAsync(JobPost jobPost)
        {
            EntityEntry<JobPost> jobPostEntityEntry = this.JobPosts.Remove(jobPost);
            await this.SaveChangesAsync();
            return jobPostEntityEntry.Entity;
        }
    }
}
