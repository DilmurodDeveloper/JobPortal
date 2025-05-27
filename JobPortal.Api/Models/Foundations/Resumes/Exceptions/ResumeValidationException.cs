namespace JobPortal.Api.Models.Foundations.Resumes.Exceptions
{
    public class ResumeValidationException : Xeption
    {
        public ResumeValidationException(Xeption innerException)
            : base(message: "Resume validation error occurred.", innerException) 
        { }
    }
}
