namespace JobPortal.Api.Models.Foundations.Applications.Exceptions
{
    public class ApplicationServiceException : Xeption
    {
        public ApplicationServiceException(Xeption innerException)
            : base(message: "Application service error occurred, contact support",
                 innerException)
        { }
    }
}
