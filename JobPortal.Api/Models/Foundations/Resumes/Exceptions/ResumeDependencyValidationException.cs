namespace JobPortal.Api.Models.Foundations.Resumes.Exceptions
{
    public class ResumeDependencyValidationException : Xeption
    {
        public ResumeDependencyValidationException(Xeption innerException)
            : base(message: "A dependency validation error occurred during resume processing.", 
                  innerException) 
        { }
    }
}
