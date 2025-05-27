namespace JobPortal.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<UserProfile> UserProfiles { get; set; } = null!;

        public async ValueTask<UserProfile> InsertUserProfileAsync(UserProfile userProfile)
        {
            EntityEntry<UserProfile> profileEntityEntry = await this.UserProfiles.AddAsync(userProfile);
            await this.SaveChangesAsync();
            return profileEntityEntry.Entity;
        }

        public IQueryable<UserProfile> SelectAllUserProfiles() =>
            this.SelectAll<UserProfile>();

        public async ValueTask<UserProfile> SelectUserProfileByIdAsync(int userProfileId)
        {
            UserProfile? profile = await this.SelectAsync<UserProfile>(userProfileId);
            return profile ?? throw new InvalidOperationException($"UserProfile with ID {userProfileId} not found.");
        }

        public async ValueTask<UserProfile?> SelectUserProfileByUserIdAsync(int userId) =>
            await this.UserProfiles.FirstOrDefaultAsync(profile => profile.UserId == userId);

        public async ValueTask<UserProfile> UpdateUserProfileAsync(UserProfile userProfile) =>
            await this.UpdateAsync(userProfile);

        public async ValueTask<UserProfile> DeleteUserProfileAsync(UserProfile userProfile) =>
            await this.DeleteAsync<UserProfile>(userProfile);
    }
}
