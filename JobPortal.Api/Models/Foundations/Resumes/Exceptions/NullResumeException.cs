namespace JobPortal.Api.Models.Foundations.Resumes.Exceptions
{
    public class NullResumeException : Xeption
    {
        public NullResumeException()
            : base(message: "The resume is null.")
        { }
    }
}
