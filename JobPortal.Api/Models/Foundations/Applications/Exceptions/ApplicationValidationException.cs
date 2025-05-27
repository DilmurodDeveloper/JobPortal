namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class ApplicationValidationException : Xeption
    {
        public ApplicationValidationException(Xeption innerException)
            : base(message: "Application validation error occurred, fix the errors and try again.", 
                  innerException: innerException) 
        { }
    }
}
