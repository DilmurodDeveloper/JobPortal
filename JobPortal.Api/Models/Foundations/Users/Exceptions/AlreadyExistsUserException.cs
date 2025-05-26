namespace JobPortal.Api.Models.Foundations.Users.Exceptions
{
    public class AlreadyExistsUserException : Exception
    {
        public AlreadyExistsUserException()
            : base("User already exists.") { }
    }
}
