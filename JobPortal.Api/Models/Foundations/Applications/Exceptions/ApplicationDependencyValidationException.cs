namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class ApplicationDependencyValidationException : Xeption
    {
        public ApplicationDependencyValidationException(Exception innerException)
            : base(message: "Application dependency validation error occurred.", 
                  innerException) 
        { }
    }
}
