namespace JobPortal.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Application> Applications { get; set; } = null!;

        public async ValueTask<Application> InsertApplicationAsync(Application application)
        {
            EntityEntry<Application> entry = await this.Applications.AddAsync(application);
            await this.SaveChangesAsync();
            return entry.Entity;
        }

        public IQueryable<Application> SelectAllApplications() =>
            this.Applications.AsQueryable();

        public async ValueTask<Application?> SelectApplicationByIdAsync(int id) =>
            await this.Applications.FindAsync(id);

        public async ValueTask<Application> UpdateApplicationAsync(Application application)
        {
            this.Applications.Update(application);
            await this.SaveChangesAsync();
            return application;
        }

        public async ValueTask<Application> DeleteApplicationAsync(Application application)
        {
            this.Applications.Remove(application);
            await this.SaveChangesAsync();
            return application;
        }
    }
}
