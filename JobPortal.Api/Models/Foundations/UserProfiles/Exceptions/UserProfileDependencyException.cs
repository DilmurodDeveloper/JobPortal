namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class UserProfileDependencyException : Xeption
    {
        public UserProfileDependencyException(Xeption innerException)
            : base(message: "User profile dependency error occurred, contact support.", 
                  innerException) 
        { }
    }
}
