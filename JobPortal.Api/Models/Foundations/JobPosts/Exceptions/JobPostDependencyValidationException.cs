namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class JobPostDependencyValidationException : Xeption
    {
        public JobPostDependencyValidationException(Xeption innerException)
            : base(message: "Job post dependency validation error occurred.", 
                  innerException) 
        { }
    }
}
