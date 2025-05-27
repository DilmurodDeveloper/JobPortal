namespace JobPortal.Api.Models.Foundations.Users.Exceptions
{
    public class UserDependencyValidationException : Xeption
    {
        public UserDependencyValidationException(Xeption innerException)
            : base(message: "User dependency validation error occurred.", 
                  innerException) 
        { }
    }
}
