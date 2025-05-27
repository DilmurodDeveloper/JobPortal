namespace JobPortal.Api.Services.Foundations.Users
{
    public partial class UserService
    {
        private delegate ValueTask<User> ReturningUserFunction();
        private delegate Task ReturningUsersFunction();

        private async ValueTask<User> TryCatch(ReturningUserFunction returningUserFunction)
        {
            try
            {
                return await returningUserFunction();
            }
            catch (NullUserException nullUserException)
            {
                throw CreateAndLogValidationException(nullUserException);
            }
            catch (InvalidUserException invalidUserException)
            {
                throw CreateAndLogValidationException(invalidUserException);
            }
            catch (SqlException sqlException)
            {
                var failedUserStorageException = new FailedUserStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedUserStorageException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var failedUserStorageException = new FailedUserStorageException(dbUpdateConcurrencyException);
                throw CreateAndLogDependencyValidationException(failedUserStorageException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedUserStorageException = new FailedUserStorageException(dbUpdateException);
                throw CreateAndLogDependencyException(failedUserStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsUserException = new AlreadyExistsUserException(duplicateKeyException);
                throw CreateAndLogDependencyValidationException(alreadyExistsUserException);
            }
            catch (NotFoundUserException notFoundUserException)
            {
                throw CreateAndLogValidationException(notFoundUserException);
            }
            catch (Exception exception)
            {
                var failedUserServiceException = new FailedUserServiceException(exception);
                throw CreateAndLogServiceException(failedUserServiceException);
            }
        }

        private async Task TryCatch(ReturningUsersFunction returningUsersFunction)
        {
            try
            {
                await returningUsersFunction();
            }
            catch (SqlException sqlException)
            {
                var failedUserStorageException = new FailedUserStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedUserStorageException);
            }
            catch (Exception exception)
            {
                var failedUserServiceException = new FailedUserServiceException(exception);
                throw CreateAndLogServiceException(failedUserServiceException);
            }
        }

        private UserValidationException CreateAndLogValidationException(Xeption exception)
        {
            var userValidationException = new UserValidationException(exception);
            this.loggingBroker.LogError(userValidationException);
            return userValidationException;
        }
        private UserDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var userDependencyException = new UserDependencyException(exception);
            this.loggingBroker.LogError(userDependencyException);
            return userDependencyException;
        }

        private UserDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var userDependencyValidationException = new UserDependencyValidationException(exception);
            this.loggingBroker.LogError(userDependencyValidationException);
            return userDependencyValidationException;
        }

        private UserDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var userDependencyException = new UserDependencyException(exception);
            this.loggingBroker.LogCritical(userDependencyException);
            return userDependencyException;
        }

        private UserServiceException CreateAndLogServiceException(Xeption exception)
        {
            var userServiceException = new UserServiceException(exception);
            this.loggingBroker.LogError(userServiceException);
            return userServiceException;
        }
    }
}
