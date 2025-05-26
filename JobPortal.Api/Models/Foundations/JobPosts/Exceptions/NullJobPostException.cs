namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class NullJobPostException : Exception
    {
        public NullJobPostException()
            : base("JobPost is null.") { }
    }
}
