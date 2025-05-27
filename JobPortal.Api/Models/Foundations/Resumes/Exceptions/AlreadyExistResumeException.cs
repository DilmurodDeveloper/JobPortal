namespace JobPortal.Api.Models.Foundations.Resume.Exceptions
{
    public class AlreadyExistResumeException : Xeption
    {
        public AlreadyExistResumeException(Exception innerException)
            : base("Resume already exists.", innerException) { }
    }
}
