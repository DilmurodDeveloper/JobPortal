namespace JobPortal.Api.Models.Foundations.Users.Exceptions
{
    public class FailedUserStorageException : Exception
    {
        public FailedUserStorageException(Exception innerException)
            : base("Failed user storage error occurred, contact support.", innerException)
        { }
    }
}
