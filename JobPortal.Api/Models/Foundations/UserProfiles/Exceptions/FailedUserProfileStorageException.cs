namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class FailedUserProfileStorageException : Xeption
    {
        public FailedUserProfileStorageException(Exception innerException)
            : base("Failed user profile storage error occurred, contact support.", 
                  innerException)
        { }
    }
}
