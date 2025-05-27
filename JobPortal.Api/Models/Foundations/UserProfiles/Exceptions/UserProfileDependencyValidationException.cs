namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class UserProfileDependencyValidationException : Xeption
    {
        public UserProfileDependencyValidationException(Xeption innerException)
            : base(message: "User profile dependency validation error occurred, fix the errors and try again.", 
                  innerException) 
        { }
    }
}
