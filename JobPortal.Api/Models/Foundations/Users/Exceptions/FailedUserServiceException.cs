namespace JobPortal.Api.Models.Foundations.Users.Exceptions
{
    public class FailedUserServiceException : Xeption
    {
        public FailedUserServiceException(Exception innerException)
            : base("Failed user service error occurred, contact support.", 
                  innerException) 
        { }
    }
}
