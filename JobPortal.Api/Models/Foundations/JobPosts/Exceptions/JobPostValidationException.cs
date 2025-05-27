namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class JobPostValidationException : Xeption
    {
        public JobPostValidationException(Xeption innerException)
            : base(message: "JobPost validation error occurred, fix the errors and try again.", 
                  innerException: innerException) 
        { }
    }
}
