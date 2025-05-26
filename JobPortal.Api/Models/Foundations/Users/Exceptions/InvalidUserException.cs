namespace JobPortal.Api.Models.Foundations.Users.Exceptions
{
    public class InvalidUserException : Exception
    {
        public InvalidUserException(string message)
            : base(message) { }
    }
}
