namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class JobPostValidationException : Exception
    {
        public JobPostValidationException(Exception innerException)
            : base("JobPost validation error occurred, fix the errors and try again.", innerException) { }
    }
}
