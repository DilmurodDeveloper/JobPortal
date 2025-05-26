namespace JobPortal.Api.Models.Foundations.Resume.Exceptions
{
    public class ResumeDependencyException : Exception
    {
        public ResumeDependencyException(Exception innerException)
            : base("A dependency error occurred during resume processing.", innerException) { }
    }
}
