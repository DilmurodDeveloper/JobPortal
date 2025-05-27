namespace JobPortal.Api.Models.Foundations.Users.Exceptions
{
    public class NotFoundUserException : Xeption
    {
        public NotFoundUserException(int userId)
            : base(message: $"Couldn't find user with id: {userId}") 
        { }
    }
}
