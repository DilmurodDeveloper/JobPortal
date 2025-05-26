namespace JobPortal.Api.Models.Foundations.Resumes.Exceptions
{
    public class ResumeValidationException : Exception
    {
        public ResumeValidationException(Exception innerException)
            : base("Resume validation error occurred.", innerException) { }
    }
}
