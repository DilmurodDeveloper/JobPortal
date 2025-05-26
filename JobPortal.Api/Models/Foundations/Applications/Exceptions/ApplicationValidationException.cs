namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class ApplicationValidationException : Exception
    {
        public ApplicationValidationException(Exception innerException)
            : base("Application validation error occurred, fix the errors and try again.", innerException) { }
    }
}
