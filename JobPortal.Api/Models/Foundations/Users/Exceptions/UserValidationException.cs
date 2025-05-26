namespace JobPortal.Api.Models.Foundations.Users.Exceptions
{
    public class UserValidationException : Exception
    {
        public UserValidationException(Exception innerException)
            : base("User validation error occurred, fix the errors and try again.", innerException) { }
    }
}
