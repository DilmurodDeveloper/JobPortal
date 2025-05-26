namespace JobPortal.Api.Models.Foundations.Resume.Exceptions
{
    public class AlreadyExistResumeException : Exception
    {
        public AlreadyExistResumeException()
            : base("Resume already exists.") { }
    }
}
