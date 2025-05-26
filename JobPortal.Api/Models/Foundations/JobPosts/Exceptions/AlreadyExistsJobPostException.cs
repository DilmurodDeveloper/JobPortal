namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class AlreadyExistsJobPostException : Exception
    {
        public AlreadyExistsJobPostException()
            : base("Job post already exists.") { }
    }
}
