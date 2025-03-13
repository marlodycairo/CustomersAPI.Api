using CustomersAPI.Api.Services.IServices;

namespace CustomersAPI.Api.Services
{
    public class EmailNotificationService(IEmailSender emailSender)
    {
        private readonly IEmailSender _emailSender = emailSender;

        public async Task NotifyUserAsync(string email, string message)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");

            await _emailSender.SendEmailAsync(email, "Notification", message);
        }
    }
}
