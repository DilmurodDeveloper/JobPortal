namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class AlreadyExistsApplicationException : Exception
    {
        public AlreadyExistsApplicationException()
            : base("Application already exists.") { }
    }
}
