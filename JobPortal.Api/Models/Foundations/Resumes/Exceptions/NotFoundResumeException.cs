namespace JobPortal.Api.Models.Foundations.Resumes.Exceptions
{
    public class NotFoundResumeException : Xeption
    {
        public NotFoundResumeException(int id)
            : base(message: $"Resume with ID {id} was not found.") 
        { }
    }
}
