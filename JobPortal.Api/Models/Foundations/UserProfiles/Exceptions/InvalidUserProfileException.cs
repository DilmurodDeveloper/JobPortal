namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class InvalidUserProfileException : Xeption
    {
        public InvalidUserProfileException()
            : base(message: "User Profile is invalid") 
        { }
    }
}
