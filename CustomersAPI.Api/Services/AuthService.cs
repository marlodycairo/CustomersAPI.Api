using CustomersAPI.Api.Services.IServices;

namespace CustomersAPI.Api.Services
{
    public class AuthService(IUserRepository userRepository)
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username and password are required");

            bool isValid = await _userRepository.UserExistsAsync(username, password);
            return isValid ? "Valid user" : "Invalid credentials";
        }
    }
}
