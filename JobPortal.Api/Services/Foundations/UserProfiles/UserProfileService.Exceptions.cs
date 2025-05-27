namespace JobPortal.Api.Services.Foundations.UserProfiles
{
    public partial class UserProfileService
    {
        private delegate ValueTask<UserProfile> ReturningUserProfileFunction();
        private delegate Task ReturningUserProfilesFunction();

        private async ValueTask<UserProfile> TryCatch(ReturningUserProfileFunction returningUserProfileFunction)
        {
            try
            {
                return await returningUserProfileFunction();
            }
            catch (NullUserProfileException nullUserProfileException)
            {
                throw CreateAndLogValidationException(nullUserProfileException);
            }
            catch (InvalidUserProfileException invalidUserProfileException)
            {
                throw CreateAndLogValidationException(invalidUserProfileException);
            }
            catch (SqlException sqlException)
            {
                var failedUserProfileStorageException = new FailedUserProfileStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedUserProfileStorageException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var failedUserProfileStorageException = new FailedUserProfileStorageException(dbUpdateConcurrencyException);
                throw CreateAndLogDependencyValidationException(failedUserProfileStorageException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedUserProfileStorageException = new FailedUserProfileStorageException(dbUpdateException);
                throw CreateAndLogDependencyException(failedUserProfileStorageException);
            }
            catch (AlreadyExistsUserProfileException alreadyExistsUserProfileException)
            {
                throw CreateAndLogDependencyValidationException(alreadyExistsUserProfileException);
            }
            catch (NotFoundUserProfileException notFoundUserProfileException)
            {
                throw CreateAndLogValidationException(notFoundUserProfileException);
            }
            catch (Exception exception)
            {
                var failedUserProfileServiceException = new FailedUserProfileServiceException(exception);
                throw CreateAndLogServiceException(failedUserProfileServiceException);
            }
        }

        private async Task TryCatch(ReturningUserProfilesFunction returningUserProfilesFunction)
        {
            try
            {
                await returningUserProfilesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedUserProfileStorageException = new FailedUserProfileStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedUserProfileStorageException);
            }
            catch (Exception exception)
            {
                var failedUserProfileServiceException = new FailedUserProfileServiceException(exception);
                throw CreateAndLogServiceException(failedUserProfileServiceException);
            }
        }

        private UserProfileValidationException CreateAndLogValidationException(Xeption exception)
        {
            var userProfileValidationException = new UserProfileValidationException(exception);
            this.loggingBroker.LogError(userProfileValidationException);
            return userProfileValidationException;
        }

        private UserProfileDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var userProfileDependencyException = new UserProfileDependencyException(exception);
            this.loggingBroker.LogError(userProfileDependencyException);
            return userProfileDependencyException;
        }

        private UserProfileDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var userProfileDependencyValidationException = new UserProfileDependencyValidationException(exception);
            this.loggingBroker.LogError(userProfileDependencyValidationException);
            return userProfileDependencyValidationException;
        }

        private UserProfileDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var userProfileDependencyException = new UserProfileDependencyException(exception);
            this.loggingBroker.LogCritical(userProfileDependencyException);
            return userProfileDependencyException;
        }

        private UserProfileServiceException CreateAndLogServiceException(Xeption exception)
        {
            var userProfileServiceException = new UserProfileServiceException(exception);
            this.loggingBroker.LogError(userProfileServiceException);
            return userProfileServiceException;
        }
    }
}
