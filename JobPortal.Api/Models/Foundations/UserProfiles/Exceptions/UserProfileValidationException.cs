namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class UserProfileValidationException : Exception
    {
        public UserProfileValidationException(Exception innerException)
            : base("User profile validation error occurred, fix the errors and try again.", innerException) { }
    }
}
