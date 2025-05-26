using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JobPortal.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Resume> Resumes { get; set; } = null!;

        public async ValueTask<Resume> InsertResumeAsync(Resume resume)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Resume> resumeEntityEntry = await broker.Resumes.AddAsync(resume);

            await broker.SaveChangesAsync();

            return resumeEntityEntry.Entity;
        }

        public IQueryable<Resume> SelectAllResumes() =>
            SelectAll<Resume>();

        public async ValueTask<Resume> SelectResumeByIdAsync(int resumeId) =>
            await SelectAsync<Resume>(resumeId);

        public async ValueTask<Resume> UpdateResumeAsync(Resume resume) =>
            await UpdateAsync(resume);

        public async ValueTask<Resume> DeleteResumeAsync(Resume resume) =>
            await DeleteAsync<Resume>(resume);
    }
}
