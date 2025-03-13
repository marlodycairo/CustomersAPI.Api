namespace CustomersAPI.Api.Services.IServices
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
