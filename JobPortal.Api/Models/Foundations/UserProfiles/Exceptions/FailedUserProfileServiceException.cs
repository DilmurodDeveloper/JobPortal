namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class FailedUserProfileServiceException : Xeption
    {
        public FailedUserProfileServiceException(Exception innerException)
            : base("Failed user profile service error occurred, contact support.", 
                  innerException) 
        { }
    }
}
