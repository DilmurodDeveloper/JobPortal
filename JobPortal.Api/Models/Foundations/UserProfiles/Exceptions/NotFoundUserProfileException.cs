namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class NotFoundUserProfileException : Exception
    {
        public NotFoundUserProfileException(int userId)
            : base($"Couldn't find user profile with user ID: {userId}.") { }
    }
}
