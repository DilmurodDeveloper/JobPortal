using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JobPortal.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Application> Applications { get; set; } = null!;

        public async ValueTask<Application> InsertApplicationAsync(Application application)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Application> appEntityEntry = await broker.Applications.AddAsync(application);

            await broker.SaveChangesAsync();

            return appEntityEntry.Entity;
        }

        public IQueryable<Application> SelectAllApplications() =>
            SelectAll<Application>();

        public async ValueTask<Application> SelectApplicationByIdAsync(int applicationId) =>
            await SelectAsync<Application>(applicationId);

        public async ValueTask<Application> UpdateApplicationAsync(Application application) =>
            await UpdateAsync(application);

        public async ValueTask<Application> DeleteApplicationAsync(Application application) =>
            await DeleteAsync<Application>(application);
    }
}
