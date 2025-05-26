namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class FailedUserProfileStorageException : Exception
    {
        public FailedUserProfileStorageException(Exception innerException)
            : base("Failed user profile storage error occurred, contact support.", innerException)
        { }
    }
}
