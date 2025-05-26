namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class NullUserProfileException : Exception
    {
        public NullUserProfileException()
            : base("User profile is null.") { }
    }
}
