namespace JobPortal.Api.Models.Foundations.JobPosts.Exceptions
{
    public class InvalidJobPostException : Xeption
    {
        public InvalidJobPostException()
            : base(message: "Job Post is invalid") 
        { }
    }
}
