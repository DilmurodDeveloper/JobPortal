using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JobPortal.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<UserProfile> UserProfiles { get; set; } = null!;

        public async ValueTask<UserProfile> InsertUserProfileAsync(UserProfile userProfile)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<UserProfile> profileEntityEntry = await broker.UserProfiles.AddAsync(userProfile);

            await broker.SaveChangesAsync();

            return profileEntityEntry.Entity;
        }

        public IQueryable<UserProfile> SelectAllUserProfiles() =>
            SelectAll<UserProfile>();

        public async ValueTask<UserProfile> SelectUserProfileByIdAsync(int userProfileId) =>
            await SelectAsync<UserProfile>(userProfileId);

        public async ValueTask<UserProfile> UpdateUserProfileAsync(UserProfile userProfile) =>
            await UpdateAsync(userProfile);

        public async ValueTask<UserProfile> DeleteUserProfileAsync(UserProfile userProfile) =>
            await DeleteAsync<UserProfile>(userProfile);
    }
}
