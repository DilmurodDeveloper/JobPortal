namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class UserProfileServiceException : Xeption
    {
        public UserProfileServiceException(Xeption innerException)
            : base(message: "User profile service error occurred, contact support.", 
                  innerException)
        { }
    }
}
