namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class NotFoundUserProfileException : Xeption
    {
        public NotFoundUserProfileException(int userId)
            : base(message: $"Couldn't find user profile with user ID: {userId}.") 
        { }
    }
}
