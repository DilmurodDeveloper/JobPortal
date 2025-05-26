namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class AlreadyExistsUserProfileException : Exception
    {
        public AlreadyExistsUserProfileException()
            : base("User profile already exists.") { }
    }
}
