namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class AlreadyExistsApplicationException : Xeption
    {
        public AlreadyExistsApplicationException(Exception innerException)
            : base(message: "Application already exists.", innerException) 
        { }
    }
}
