namespace CustomersAPI.Api.Services.IServices
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(string username, string password);
    }
}
