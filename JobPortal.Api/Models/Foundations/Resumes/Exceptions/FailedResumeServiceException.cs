namespace JobPortal.Api.Models.Foundations.Resume.Exceptions
{
    public class FailedResumeServiceException : Exception
    {
        public FailedResumeServiceException(Exception innerException)
            : base("Resume service failed.", innerException) { }
    }
}
