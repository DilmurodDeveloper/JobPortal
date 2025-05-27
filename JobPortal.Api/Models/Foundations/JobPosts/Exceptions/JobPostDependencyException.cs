namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class JobPostDependencyException : Xeption
    {
        public JobPostDependencyException(Xeption innerException)
            : base(message: "Job post dependency error occurred, contact support.", 
                  innerException) 
        { }
    }
}
