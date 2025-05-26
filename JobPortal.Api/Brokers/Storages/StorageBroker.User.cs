using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JobPortal.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<User> Users { get; set; } = null!;

        public async ValueTask<User> InsertUserAsync(User user)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<User> userEntityEntry = await broker.Users.AddAsync(user);

            await broker.SaveChangesAsync();

            return userEntityEntry.Entity;
        }

        public IQueryable<User> SelectAllUsers() =>
            SelectAll<User>();

        public async ValueTask<User> SelectUserByIdAsync(int userId) =>
            await SelectAsync<User>(userId);

        public async ValueTask<User> UpdateUserAsync(User user) =>
            await UpdateAsync(user);

        public async ValueTask<User> DeleteUserAsync(User user) =>
            await DeleteAsync<User>(user);
    }
}
