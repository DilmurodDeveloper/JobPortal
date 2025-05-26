namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class InvalidUserProfileException : Exception
    {
        public InvalidUserProfileException(string message)
            : base(message) { }
    }
}
