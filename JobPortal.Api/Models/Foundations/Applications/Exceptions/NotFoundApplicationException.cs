namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class NotFoundApplicationException : Exception
    {
        public NotFoundApplicationException(Guid applicationId)
            : base($"Couldn't find application with id: {applicationId}") { }
    }
}
