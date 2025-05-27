namespace JobPortal.Api.Models.Foundations.Users.Exceptions
{
    public class AlreadyExistsUserException : Xeption
    {
        public AlreadyExistsUserException(Exception innerException)
            : base("User already exists.", innerException) 
        { }
    }
}
