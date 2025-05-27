namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class AlreadyExistsJobPostException : Xeption
    {
        public AlreadyExistsJobPostException(Exception innerException)
            : base(message: "Job post already exists.", innerException) 
        { }
    }
}
