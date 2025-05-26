namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class InvalidJobPostException : Exception
    {
        public InvalidJobPostException(string message)
            : base(message) { }
    }
}
