namespace JobPortal.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Resume> Resumes { get; set; } = null!;

        public async ValueTask<Resume> InsertResumeAsync(Resume resume)
        {
            EntityEntry<Resume> resumeEntityEntry = await this.Resumes.AddAsync(resume);

            await this.SaveChangesAsync();

            return resumeEntityEntry.Entity;
        }

        public IQueryable<Resume> SelectAllResumes() =>
            SelectAll<Resume>();

        public async ValueTask<Resume> SelectResumeByIdAsync(int resumeId)
        {
            Resume? resume = await SelectAsync<Resume>(resumeId);
            return resume ?? throw new InvalidOperationException($"Resume with ID {resumeId} not found.");
        }

        public async ValueTask<Resume?> SelectResumeBySeekerIdAsync(int userId) =>
            await this.Resumes.FirstOrDefaultAsync(r => r.SeekerId == userId);

        public async ValueTask<Resume?> SelectResumeByIdAndSeekerIdAsync(int resumeId, int userId) =>
            await this.Resumes.FirstOrDefaultAsync(r => r.Id == resumeId && r.SeekerId == userId);

        public async ValueTask<Resume> UpdateResumeAsync(Resume resume) =>
            await UpdateAsync(resume);

        public async ValueTask<Resume> DeleteResumeAsync(Resume resume) =>
            await DeleteAsync<Resume>(resume);
    }
}
