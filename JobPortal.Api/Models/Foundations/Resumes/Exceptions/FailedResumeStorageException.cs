namespace JobPortal.Api.Models.Foundations.Resume.Exceptions
{
    public class FailedResumeStorageException : Xeption
    {
        public FailedResumeStorageException(Exception innerException)
            : base("Resume storage operation failed.", innerException) 
        { }
    }
}
