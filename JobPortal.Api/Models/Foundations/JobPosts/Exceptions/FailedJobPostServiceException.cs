namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class FailedJobPostServiceException : Xeption
    {
        public FailedJobPostServiceException(Exception innerException)
            : base(message: "Failed job post service error occurred, contact support.", 
                  innerException) 
        { }
    }
}
