namespace JobPortal.Api.Models.Foundations.Resumes.Exceptions
{
    public class ResumeServiceException : Xeption
    {
        public ResumeServiceException(Xeption innerException)
            : base(message: "An unexpected error occurred in the resume service.", 
                  innerException) 
        { }
    }
}
