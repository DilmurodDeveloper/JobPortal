namespace JobPortal.Api.Models.Foundations.Users.Exceptions
{
    public class FailedUserStorageException : Xeption
    {
        public FailedUserStorageException(Exception innerException)
            : base("Failed user storage error occurred, contact support.", 
                  innerException)
        { }
    }
}
