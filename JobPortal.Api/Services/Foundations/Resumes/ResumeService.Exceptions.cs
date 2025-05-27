namespace JobPortal.Api.Services.Foundations.Resumes
{
    public partial class ResumeService
    {
        private delegate ValueTask<Resume> ReturningResumeFunction();
        private delegate Task ReturningResumesFunction();

        private async ValueTask<Resume> TryCatch(ReturningResumeFunction returningResumeFunction)
        {
            try
            {
                return await returningResumeFunction();
            }
            catch (NotFoundResumeException notFoundResumeException)
            {
                throw CreateAndLogValidationException(notFoundResumeException);
            }
            catch (InvalidResumeException invalidResumeException)
            {
                throw CreateAndLogValidationException(invalidResumeException);
            }
            catch (NullResumeException nullResumeException)
            {
                throw CreateAndLogValidationException(nullResumeException);
            }
            catch (ResumeValidationException resumeValidationException)
            {
                throw CreateAndLogValidationException(resumeValidationException);
            }
            catch (SqlException sqlException)
            {
                var failedStorageException = new FailedResumeStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedStorageException);
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                var failedStorageException = new FailedResumeStorageException(concurrencyException);
                throw CreateAndLogDependencyValidationException(failedStorageException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedStorageException = new FailedResumeStorageException(dbUpdateException);
                throw CreateAndLogDependencyException(failedStorageException);
            }
            catch (Exception exception)
            {
                var failedServiceException = new FailedResumeServiceException(exception);
                throw CreateAndLogServiceException(failedServiceException);
            }
        }

        private async Task TryCatch(ReturningResumesFunction returningResumesFunction)
        {
            try
            {
                await returningResumesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedStorageException = new FailedResumeStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedStorageException);
            }
            catch (Exception exception)
            {
                var failedServiceException = new FailedResumeServiceException(exception);
                throw CreateAndLogServiceException(failedServiceException);
            }
        }

        private ResumeValidationException CreateAndLogValidationException(Exception exception)
        {
            var validationException = new ResumeValidationException(new Xeption(exception.Message, exception));
            _loggingBroker.LogError(validationException);
            return validationException;
        }

        private ResumeDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var resumeDependencyException = new ResumeDependencyException(new Xeption(exception.Message, exception));
            _loggingBroker.LogError(resumeDependencyException);
            return resumeDependencyException;
        }

        private ResumeDependencyValidationException CreateAndLogDependencyValidationException(Exception exception)
        {
            var resumeDependencyValidationException = new ResumeDependencyValidationException(new Xeption(exception.Message, exception));
            _loggingBroker.LogError(resumeDependencyValidationException);
            return resumeDependencyValidationException;
        }

        private ResumeDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var resumeDependencyException = new ResumeDependencyException(new Xeption(exception.Message, exception));
            _loggingBroker.LogCritical(resumeDependencyException);
            return resumeDependencyException;
        }

        private ResumeServiceException CreateAndLogServiceException(Exception exception)
        {
            var resumeServiceException = new ResumeServiceException(new Xeption(exception.Message, exception));
            _loggingBroker.LogError(resumeServiceException);
            return resumeServiceException;
        }
    }
}
