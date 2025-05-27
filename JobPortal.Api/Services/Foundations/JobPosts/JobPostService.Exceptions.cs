namespace JobPortal.Api.Services.Foundations.JobPosts
{
    public partial class JobPostService
    {
        private delegate ValueTask<JobPost> ReturningJobPostFunction();
        private delegate Task ReturningJobPostsFunction();

        private async ValueTask<JobPost> TryCatch(ReturningJobPostFunction returningJobPostFunction)
        {
            try
            {
                return await returningJobPostFunction();
            }
            catch (NotFoundJobPostException notFoundJobPostException)
            {
                throw CreateAndLogValidationException(notFoundJobPostException);
            }
            catch (InvalidJobPostException invalidJobPostException)
            {
                throw CreateAndLogValidationException(invalidJobPostException);
            }
            catch (NullJobPostException nullJobPostException)
            {
                throw CreateAndLogValidationException(nullJobPostException);
            }
            catch (JobPostValidationException jobPostValidationException)
            {
                throw CreateAndLogValidationException(jobPostValidationException);
            }
            catch (SqlException sqlException)
            {
                var failedStorageException = new FailedJobPostStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedStorageException);
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                var failedStorageException = new FailedJobPostStorageException(concurrencyException);
                throw CreateAndLogDependencyValidationException(failedStorageException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedStorageException = new FailedJobPostStorageException(dbUpdateException);
                throw CreateAndLogDependencyException(failedStorageException);
            }
            catch (Exception exception)
            {
                var failedServiceException = new FailedJobPostServiceException(exception);
                throw CreateAndLogServiceException(failedServiceException);
            }
        }

        private async Task TryCatch(ReturningJobPostsFunction returningJobPostsFunction)
        {
            try
            {
                await returningJobPostsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedStorageException = new FailedJobPostStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedStorageException);
            }
            catch (Exception exception)
            {
                var failedServiceException = new FailedJobPostServiceException(exception);
                throw CreateAndLogServiceException(failedServiceException);
            }
        }

        private JobPostValidationException CreateAndLogValidationException(Exception exception)
        {
            var validationException = new JobPostValidationException(new Xeption(exception.Message, exception));
            this.loggingBroker.LogError(validationException);
            return validationException;
        }

        private JobPostDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var jobPostDependencyException = new JobPostDependencyException(new Xeption(exception.Message, exception));
            this.loggingBroker.LogError(jobPostDependencyException);
            return jobPostDependencyException;
        }

        private JobPostDependencyValidationException CreateAndLogDependencyValidationException(Exception exception)
        {
            var jobPostDependencyValidationException = new JobPostDependencyValidationException(new Xeption(exception.Message, exception));
            this.loggingBroker.LogError(jobPostDependencyValidationException);
            return jobPostDependencyValidationException;
        }

        private JobPostDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var jobPostDependencyException = new JobPostDependencyException(new Xeption(exception.Message, exception));
            this.loggingBroker.LogCritical(jobPostDependencyException);
            return jobPostDependencyException;
        }

        private JobPostServiceException CreateAndLogServiceException(Exception exception)
        {
            var jobPostServiceException = new JobPostServiceException(new Xeption(exception.Message, exception));
            this.loggingBroker.LogError(jobPostServiceException);
            return jobPostServiceException;
        }
    }
}
