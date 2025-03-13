using CustomersAPI.Api.Services;
using CustomersAPI.Api.Services.IServices;
using Moq;

namespace CustomersAPI.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _authService = new AuthService(_mockUserRepo.Object);
        }

        [Fact]
        public async Task ValidateUser_WhenUsernameAndPasswordIsValid()
        {
            string username = "dominique2025";
            string pass = "evergreen";
            string message = "Valid user";

            _mockUserRepo.Setup(x => x.UserExistsAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var response = await _authService.AuthenticateAsync(username, pass);

            Assert.Equal(message, response);

            _mockUserRepo.Verify(x => x.UserExistsAsync(username, pass), Times.Once);
        }

        [Fact]
        public async Task ValidateUser_WhenUsernameAndPasswordIsNotValid()
        {
            string username = "dominique2025";
            string pass = "evergreen";
            string message = "Invalid credentials";

            _mockUserRepo.Setup(x => x.UserExistsAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var response = await _authService.AuthenticateAsync(username, pass);

            Assert.Equal(message, response);

            _mockUserRepo.Verify(x => x.UserExistsAsync(username, pass), Times.Once);
        }

        [Fact]
        public async Task ValidateUser_ThrowsArgumentException_WhenUsernameAndPasswordIsNullOrEmpty()
        {
            string username = "";
            string pass = "";

            await Assert.ThrowsAsync<ArgumentException>(() => _authService.AuthenticateAsync(username, pass));

            _mockUserRepo.Verify(x => x.UserExistsAsync(username, pass), Times.Never);
        }

        [Fact]
        public async Task ValidateUser_ThrowsArgumentException_WhenUsernameOrPasswordIsNullOrEmpty()
        {
            string username = "dominique2025";
            string pass = "";

            await Assert.ThrowsAsync<ArgumentException>(() => _authService.AuthenticateAsync(username, pass));

            _mockUserRepo.Verify(x => x.UserExistsAsync(username, pass), Times.Never);
        }

        // tests con feedback
        [Fact]
        public async Task ValidateUser_ReturnsValidMessage_WhenCredentialsAreCorrect()
        {
            string username = "dominique2025";
            string pass = "evergreen";

            _mockUserRepo
                .Setup(x => x.UserExistsAsync(username, pass))
                .ReturnsAsync(true);

            var response = await _authService.AuthenticateAsync(username, pass);

            Assert.Equal("Valid user", response);
            _mockUserRepo.Verify(x => x.UserExistsAsync(username, pass), Times.Once);
        }

        [Fact]
        public async Task ValidateUser_ReturnsInvalidMessage_WhenCredentialsAreIncorrect()
        {
            string username = "dominique2025";
            string pass = "wrongpass";

            _mockUserRepo
                .Setup(x => x.UserExistsAsync(username, pass))
                .ReturnsAsync(false);

            var response = await _authService.AuthenticateAsync(username, pass);

            Assert.Equal("Invalid credentials", response);
            _mockUserRepo.Verify(x => x.UserExistsAsync(username, pass), Times.Once);
        }

        [Theory]
        [InlineData(null, "password")]
        [InlineData("username", null)]
        [InlineData("", "password")]
        [InlineData("username", "")]
        [InlineData(null, null)]
        [InlineData("", "")]
        public async Task ValidateUser_ThrowsArgumentException_WhenUsernameOrPasswordIsNullOrEmpty2(string username, string pass)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _authService.AuthenticateAsync(username, pass));

            _mockUserRepo.Verify(x => x.UserExistsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
