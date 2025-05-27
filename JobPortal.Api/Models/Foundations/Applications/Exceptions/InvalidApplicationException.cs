namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class InvalidApplicationException : Xeption
    {
        public InvalidApplicationException()
            : base(message: "Application is invalid") 
        { }
    }
}
