namespace JobPortal.Api.Models.Foundations.Resumes.Exceptions
{
    public class InvalidResumeException : Exception
    {
        public InvalidResumeException(string message)
            : base(message) { }
    }
}
