namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class JobPostDependencyException : Exception
    {
        public JobPostDependencyException(Exception innerException)
            : base("Job post dependency error occurred, contact support.", innerException) { }
    }
}
