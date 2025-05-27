namespace JobPortal.Api.Models.Foundations.Resumes.Exceptions
{
    public class InvalidResumeException : Xeption
    {
        public InvalidResumeException()
            : base(message: "Resume is invalid") { }
    }
}
