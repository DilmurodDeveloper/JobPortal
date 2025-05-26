namespace JobPortal.Api.Models.Foundations.Resumes.Exceptions
{
    public class ResumeDependencyValidationException : Exception
    {
        public ResumeDependencyValidationException(Exception innerException)
            : base("A dependency validation error occurred during resume processing.", innerException) { }
    }
}
