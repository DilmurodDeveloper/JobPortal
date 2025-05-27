namespace JobPortal.Api.Models.Foundations.Resume.Exceptions
{
    public class FailedResumeServiceException : Xeption
    {
        public FailedResumeServiceException(Exception innerException)
            : base("Resume service failed.", innerException) 
        { }
    }
}
