namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class InvalidApplicationException : Exception
    {
        public InvalidApplicationException(string message)
            : base(message) { }
    }
}
