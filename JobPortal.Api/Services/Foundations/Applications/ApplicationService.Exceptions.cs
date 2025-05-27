namespace JobPortal.Api.Services.Foundations.Applications
{
    public partial class ApplicationService
    {
        private delegate ValueTask<Application> ReturningApplicationFunction();
        private delegate IQueryable<Application> ReturningApplicationsFunction();

        private async ValueTask<Application> TryCatch(ReturningApplicationFunction returningApplicationFunction)
        {
            try
            {
                return await returningApplicationFunction();
            }
            catch (NullApplicationException nullApplicationException)
            {
                throw CreateAndLogValidationException(nullApplicationException);
            }
            catch (InvalidApplicationException invalidApplicationException)
            {
                throw CreateAndLogValidationException(invalidApplicationException);
            }
            catch (SqlException sqlException)
            {
                var failedApplicationStorageException = new FailedApplicationStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedApplicationStorageException);
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                var failedApplicationStorageException = new FailedApplicationStorageException(concurrencyException);
                throw CreateAndLogDependencyValidationException(failedApplicationStorageException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedApplicationStorageException = new FailedApplicationStorageException(dbUpdateException);
                throw CreateAndLogDependencyException(failedApplicationStorageException);
            }
            catch (NotFoundApplicationException notFoundApplicationException)
            {
                throw CreateAndLogValidationException(notFoundApplicationException);
            }
            catch (Exception exception)
            {
                var failedApplicationServiceException = new FailedApplicationServiceException(exception);
                throw CreateAndLogServiceException(failedApplicationServiceException);
            }
        }

        private IQueryable<Application> TryCatch(ReturningApplicationsFunction returningApplicationsFunction)
        {
            try
            {
                return returningApplicationsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedApplicationStorageException = new FailedApplicationStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedApplicationStorageException);
            }
            catch (Exception exception)
            {
                var failedApplicationServiceException = new FailedApplicationServiceException(exception);
                throw CreateAndLogServiceException(failedApplicationServiceException);
            }
        }

        private ApplicationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var applicationValidationException = new ApplicationValidationException(exception);
            this.loggingBroker.LogError(applicationValidationException);
            return applicationValidationException;
        }

        private ApplicationDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var applicationDependencyException = new ApplicationDependencyException(exception);
            this.loggingBroker.LogError(applicationDependencyException);
            return applicationDependencyException;
        }

        private ApplicationDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var applicationDependencyValidationException = new ApplicationDependencyValidationException(exception);
            this.loggingBroker.LogError(applicationDependencyValidationException);
            return applicationDependencyValidationException;
        }

        private ApplicationDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var applicationDependencyException = new ApplicationDependencyException(exception);
            this.loggingBroker.LogCritical(applicationDependencyException);
            return applicationDependencyException;
        }

        private ApplicationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var applicationServiceException = new ApplicationServiceException(exception);
            this.loggingBroker.LogError(applicationServiceException);
            return applicationServiceException;
        }
    }
}
