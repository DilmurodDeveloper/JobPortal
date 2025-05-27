namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class NullApplicationException : Xeption
    {
        public NullApplicationException()
            : base(message: "Application is null.") 
        { }
    }
}
