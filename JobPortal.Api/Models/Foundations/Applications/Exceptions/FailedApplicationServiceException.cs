namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class FailedApplicationServiceException : Exception
    {
        public FailedApplicationServiceException(Exception innerException)
            : base("Failed application service error occurred, contact support.", innerException) { }
    }
}
