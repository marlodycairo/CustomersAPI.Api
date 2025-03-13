using CustomersAPI.Api.Models;
using CustomersAPI.Api.Services.IServices;
using CustomersAPI.Api.Utils;

namespace CustomersAPI.Api.Services
{
    public class ProductExternalApiService : IProductExternalApiService
    {
        private readonly IUtilities<ProductResponse> _products;

        public ProductExternalApiService(IUtilities<ProductResponse> products)
        {
            _products = products;
        }

        public async Task<GenericResponse<IList<ProductResponse>>> SendRequestAsync(string url)
        {
            var products = new GenericResponse<IList<ProductResponse>>();

            var responses = await _products.ExternalServiceUtility(url);

            products.Result = responses.Result;

            return products;
        }
    }
}
