namespace JobPortal.Api.Models.Foundations.Users.Exceptions
{
    public class UserDependencyException : Exception
    {
        public UserDependencyException(Exception innerException)
            : base("User dependency error occurred, contact support.", innerException) { }
    }
}
