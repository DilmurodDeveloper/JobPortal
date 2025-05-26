namespace JobPortal.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<UserProfile> InsertUserProfileAsync(UserProfile profile);
        IQueryable<UserProfile> SelectAllUserProfiles();
        ValueTask<UserProfile> SelectUserProfileByIdAsync(int profileId);
        ValueTask<UserProfile> UpdateUserProfileAsync(UserProfile profile);
        ValueTask<UserProfile> DeleteUserProfileAsync(UserProfile profile);
    }
}
