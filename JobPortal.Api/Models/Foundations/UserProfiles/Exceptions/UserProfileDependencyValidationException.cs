namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class UserProfileDependencyValidationException : Exception
    {
        public UserProfileDependencyValidationException(Exception innerException)
            : base("User profile dependency validation error occurred, fix the errors and try again.", innerException) { }
    }
}
