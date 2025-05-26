namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class JobPostDependencyValidationException : Exception
    {
        public JobPostDependencyValidationException(Exception innerException)
            : base("Job post dependency validation error occurred.", innerException) { }
    }
}
