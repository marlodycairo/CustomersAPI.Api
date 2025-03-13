using CustomersAPI.Api.Services;
using CustomersAPI.Api.Services.IServices;
using Moq;

namespace CustomersAPI.Tests.Services
{
    public class EmailNotificationServiceTests
    {
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly EmailNotificationService _emailNotificationService;

        public EmailNotificationServiceTests()
        {
            _mockEmailSender = new Mock<IEmailSender>();

            _emailNotificationService = new EmailNotificationService(_mockEmailSender.Object);
        }

        [Fact]
        public async Task NotifyUser_ThrowsArgumentException_WhenEmailIsNullOrEmpty()
        {
            string message = "Sending message";

            await Assert.ThrowsAsync<ArgumentException>(() => _emailNotificationService.NotifyUserAsync(null, message));
            await Assert.ThrowsAsync<ArgumentException>(() => _emailNotificationService.NotifyUserAsync("", message));

            _mockEmailSender.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async Task NotifyUser_SendsEmail_WhenEmailIsValid()
        {
            string email = "zucaritas@correo.com";
            string message = "Sending message";

            await _emailNotificationService.NotifyUserAsync(email, message);

            _mockEmailSender.Verify(x => x.SendEmailAsync(email, "Notification", message), Times.Once());
        }
    }
}
