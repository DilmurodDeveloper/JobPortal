namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class NotFoundJobPostException : Exception
    {
        public NotFoundJobPostException(Guid jobPostId)
            : base($"Couldn't find job post with id: {jobPostId}") { }
    }
}
