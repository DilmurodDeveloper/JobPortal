namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class NotFoundJobPostException : Xeption
    {
        public NotFoundJobPostException(int jobPostId)
            : base(message: $"Couldn't find job post with id: {jobPostId}") 
        { }
    }
}
