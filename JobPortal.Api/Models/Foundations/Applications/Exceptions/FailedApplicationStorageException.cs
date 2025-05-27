namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class FailedApplicationStorageException : Xeption
    {
        public FailedApplicationStorageException(Exception innerException)
            : base(message: "Failed application storage error occurred, contact support.", 
                  innerException)
        { }
    }
}
