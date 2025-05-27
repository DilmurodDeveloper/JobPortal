namespace JobPortal.Api.Models.Foundations.Resume.Exceptions
{
    public class ResumeDependencyException : Xeption
    {
        public ResumeDependencyException(Xeption innerException)
            : base(message: "A dependency error occurred during resume processing.", 
                  innerException) 
        { }
    }
}
