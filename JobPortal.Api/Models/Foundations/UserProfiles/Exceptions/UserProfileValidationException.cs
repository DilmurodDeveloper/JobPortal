namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class UserProfileValidationException : Xeption
    {
        public UserProfileValidationException(Xeption innerException)
            : base(message: "User profile validation error occurred, fix the errors and try again.", 
                  innerException: innerException) 
        { }
    }
}
