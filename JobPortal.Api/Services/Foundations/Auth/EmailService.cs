namespace JobPortal.Api.Services.Foundations.Auth
{
    public class EmailService : IEmailService
    {
        public Task SendAsync(string to, string subject, string body)
        {
            return Task.CompletedTask;
        }
    }
}
