namespace JobPortal.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<JobPost> InsertJobPostAsync(JobPost jobPost);
        IQueryable<JobPost> SelectAllJobPosts();
        ValueTask<JobPost> SelectJobPostByIdAsync(int jobPostId);
        ValueTask<JobPost> UpdateJobPostAsync(JobPost jobPost);
        ValueTask<JobPost> DeleteJobPostAsync(JobPost jobPost);
    }
}
