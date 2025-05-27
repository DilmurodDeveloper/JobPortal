namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class NullUserProfileException : Xeption
    {
        public NullUserProfileException()
            : base(message: "User profile is null.") 
        { }
    }
}
