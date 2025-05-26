namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class NullApplicationException : Exception
    {
        public NullApplicationException()
            : base("Application is null.") { }
    }
}
