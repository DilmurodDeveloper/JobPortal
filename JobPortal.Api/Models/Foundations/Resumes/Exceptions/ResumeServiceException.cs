namespace JobPortal.Api.Models.Foundations.Resumes.Exceptions
{
    public class ResumeServiceException : Exception
    {
        public ResumeServiceException(Exception innerException)
            : base("An unexpected error occurred in the resume service.", innerException) { }
    }
}
