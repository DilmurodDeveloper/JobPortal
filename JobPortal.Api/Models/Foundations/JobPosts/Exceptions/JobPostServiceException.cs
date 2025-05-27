namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class JobPostServiceException : Xeption
    {
        public JobPostServiceException(Xeption innerException)
            : base(message: "Job post service error occurred, contact support",
                 innerException)
        { }
    }
}
