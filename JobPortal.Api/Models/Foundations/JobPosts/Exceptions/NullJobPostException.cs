namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class NullJobPostException : Xeption
    {
        public NullJobPostException()
            : base(message: "JobPost is null.") 
        { }
    }
}
