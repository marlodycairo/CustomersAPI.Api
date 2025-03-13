using CustomersAPI.Api.Models;

namespace CustomersAPI.Api.Services.IServices
{
    public interface IProductExternalApiService
    {
        public Task<GenericResponse<IList<ProductResponse>>> SendRequestAsync(string url);
    }
}
