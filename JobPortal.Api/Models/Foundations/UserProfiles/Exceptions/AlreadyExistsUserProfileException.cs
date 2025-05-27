namespace JobPortal.Api.Models.Foundations.UserProfiles.Exceptions
{
    public class AlreadyExistsUserProfileException : Xeption
    {
        public AlreadyExistsUserProfileException(Exception innerException)
            : base("User profile already exists.", innerException) 
        { }
    }
}
