namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class FailedApplicationServiceException : Xeption
    {
        public FailedApplicationServiceException(Exception innerException)
            : base(message: "Failed application service error occurred, contact support.", 
                  innerException) 
        { }
    }
}
