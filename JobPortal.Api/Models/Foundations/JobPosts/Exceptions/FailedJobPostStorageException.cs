namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class FailedJobPostStorageException : Xeption
    {
        public FailedJobPostStorageException(Exception innerException)
            : base(message: "Failed job post storage error occurred, contact support.", 
                  innerException)
        { }
    }
}
