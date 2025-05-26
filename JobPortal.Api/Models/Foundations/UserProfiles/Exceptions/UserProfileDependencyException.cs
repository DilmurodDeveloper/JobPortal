namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class UserProfileDependencyException : Exception
    {
        public UserProfileDependencyException(Exception innerException)
            : base("User profile dependency error occurred, contact support.", innerException) { }
    }
}
