namespace JobPortal.Api.Models.Foundations.Users.Exceptions
{
    public class NotFoundUserException : Exception
    {
        public NotFoundUserException(int userId)
            : base($"Couldn't find user with id: {userId}") { }
    }
}
