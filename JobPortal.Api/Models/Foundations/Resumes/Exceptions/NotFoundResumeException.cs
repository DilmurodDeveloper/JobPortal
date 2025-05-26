namespace JobPortal.Api.Models.Foundations.Resumes.Exceptions
{
    public class NotFoundResumeException : Exception
    {
        public NotFoundResumeException(int id)
            : base($"Resume with ID {id} was not found.") { }
    }
}
