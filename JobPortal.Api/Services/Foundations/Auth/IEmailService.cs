namespace JobPortal.Api.Services.Foundations.Auth
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
    }
}
