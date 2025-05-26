namespace JobPortal.Api.Models.Foundations.Users.Exceptions
{
    public class NullUserException : Exception
    {
        public NullUserException()
            : base("User is null.") { }
    }
}
