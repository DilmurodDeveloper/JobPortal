namespace JobPortal.Api.Services.Foundations.UserProfiles
{
    public partial class UserProfileService : IUserProfileService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public UserProfileService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public async Task<UserProfileDto?> GetProfileAsync(int userId) =>
            await TryCatch(async () =>
            {
                var profile = await storageBroker.SelectUserProfileByUserIdAsync(userId);

                if (profile == null)
                {
                    return null;
                }

                return new UserProfileDto
                {
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    PhoneNumber = profile.PhoneNumber,
                    Address = profile.Address,
                    Bio = profile.Bio,
                    ProfilePictureUrl = profile.ProfilePictureUrl,
                    Location = profile.Location
                };
            });

        public async Task<bool> UpdateProfileAsync(int userId, UserProfileDto dto) =>
            await TryCatch(async () =>
            {
                UserProfile? profile = await storageBroker.SelectUserProfileByUserIdAsync(userId);

                if (profile == null)
                {
                    return false;
                }

                profile.FirstName = dto.FirstName;
                profile.LastName = dto.LastName;
                profile.PhoneNumber = dto.PhoneNumber;
                profile.Address = dto.Address;
                profile.Bio = dto.Bio;
                profile.ProfilePictureUrl = dto.ProfilePictureUrl;
                profile.Location = dto.Location;
                profile.UpdatedAt = DateTime.UtcNow;

                await storageBroker.UpdateUserProfileAsync(profile);

                return true;
            });

        public async Task<bool> DeleteProfileAsync(int userId) =>
            await TryCatch(async () =>
            {
                UserProfile? profile = await storageBroker.SelectUserProfileByUserIdAsync(userId);

                if (profile == null)
                {
                    return false;
                }

                await storageBroker.DeleteUserProfileAsync(profile);

                return true;
            });


        private async Task<T> TryCatch<T>(Func<Task<T>> returningFunction)
        {
            try
            {
                return await returningFunction();
            }
            catch (Exception exception)
            {
                var failedUserProfileServiceException =
                    new FailedUserProfileServiceException(exception);

                loggingBroker.LogError(failedUserProfileServiceException);

                throw failedUserProfileServiceException;
            }
        }
    }
}
