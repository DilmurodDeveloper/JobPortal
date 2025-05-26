namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class FailedJobPostServiceException : Exception
    {
        public FailedJobPostServiceException(Exception innerException)
            : base("Failed job post service error occurred, contact support.", innerException) { }
    }
}
