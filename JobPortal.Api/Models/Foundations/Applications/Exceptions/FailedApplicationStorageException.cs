namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class FailedApplicationStorageException : Exception
    {
        public FailedApplicationStorageException(Exception innerException)
            : base("Failed application storage error occurred, contact support.", innerException)
        { }
    }
}
