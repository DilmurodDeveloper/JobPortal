namespace JobPortal.Api.Brokers.Storages
{
    public partial class StorageBroker : DbContext, IStorageBroker
    {
        private readonly IConfiguration configuration;

        public StorageBroker(DbContextOptions<StorageBroker> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
            this.Database.Migrate();
        }


        public async ValueTask<T> InsertAsync<T>(T entity) where T : class
        {
            this.Set<T>().Add(entity);
            await this.SaveChangesAsync();
            return entity;
        }

        public IQueryable<T> SelectAll<T>() where T : class =>
            this.Set<T>().AsNoTracking();

        public async ValueTask<T?> SelectAsync<T>(params object[] keyValues) where T : class =>
            await this.Set<T>().FindAsync(keyValues);

        public async ValueTask<T> UpdateAsync<T>(T entity) where T : class
        {
            this.Set<T>().Update(entity);
            await this.SaveChangesAsync();
            return entity;
        }

        public async ValueTask<T> DeleteAsync<T>(T entity) where T : class
        {
            this.Set<T>().Remove(entity);
            await this.SaveChangesAsync();
            return entity;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = this.configuration.GetConnectionString("DefaultConnection")!;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public override void Dispose() { }
    }
}
