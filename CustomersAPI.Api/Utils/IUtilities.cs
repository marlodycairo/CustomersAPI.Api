using CustomersAPI.Api.Models;

namespace CustomersAPI.Api.Utils
{
    public interface IUtilities<T> where T : class
    {
        Task<IList<T>> CallToConsumeWebService(string url);
        Task<string> ConsumeServiceDeleteAsync(string url);
        Task<string> ConsumeServiceGetAsync(string url);
        Task<string> ConsumeServicePostAsync(string url, HttpContent content);
        Task<string> ConsumeServiceUpdateAsync(string url, HttpContent content);
        Task<GenericResponse<IList<T>>> ExternalServiceUtility(string url);
        Task<GenericResponse<T>> ExternalServiceUtilityByEntity(string url);
        Task<GenericResponse<T>> ExternalServiceUtilityByEntityById(string url);
        Task<GenericResponse<T>> ExternalServiceUtilityCreate(string url, HttpContent content);
        Task<GenericResponse<T>> ExternalServiceUtilityDelete(string url);
        Task<GenericResponse<T>> ExternalServiceUtilityUpdate(string url, HttpContent content);
    }
}