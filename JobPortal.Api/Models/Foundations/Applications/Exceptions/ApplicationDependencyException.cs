namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class ApplicationDependencyException : Xeption
    {
        public ApplicationDependencyException(Exception innerException)
            : base(message: "Application dependency error occurred, contact support.", 
                  innerException) 
        { }
    }
}
