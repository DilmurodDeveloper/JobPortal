namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class ApplicationDependencyException : Exception
    {
        public ApplicationDependencyException(Exception innerException)
            : base("Application dependency error occurred, contact support.", innerException) { }
    }
}
