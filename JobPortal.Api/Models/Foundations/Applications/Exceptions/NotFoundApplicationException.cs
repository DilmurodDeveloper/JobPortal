namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class NotFoundApplicationException : Xeption
    {
        public NotFoundApplicationException(int applicationId)
            : base(message: $"Couldn't find application with id: {applicationId}") 
        { }
    }
}
