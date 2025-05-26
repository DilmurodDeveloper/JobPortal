namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class ApplicationDependencyValidationException : Exception
    {
        public ApplicationDependencyValidationException(Exception innerException)
            : base("Application dependency validation error occurred.", innerException) { }
    }
}
