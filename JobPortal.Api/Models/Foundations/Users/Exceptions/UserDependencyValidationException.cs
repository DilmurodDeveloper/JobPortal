namespace JobPortal.Api.Models.Foundations.Users.Exceptions
{
    public class UserDependencyValidationException : Exception
    {
        public UserDependencyValidationException(Exception innerException)
            : base("User dependency validation error occurred.", innerException) { }
    }
}
