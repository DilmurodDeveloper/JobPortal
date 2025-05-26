namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class FailedJobPostStorageException : Exception
    {
        public FailedJobPostStorageException(Exception innerException)
            : base("Failed job post storage error occurred, contact support.", innerException)
        { }
    }
}
