using Newtonsoft.Json;
using Polly.Retry;
using Polly;
using CustomersAPI.Api.Models;

namespace CustomersAPI.Api.Utils
{
    public class Utilities<T> : IUtilities<T> where T : class
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Utilities(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GenericResponse<IList<T>>> ExternalServiceUtility(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url), "El parámetro url no puede ser nulo o vacío.");
            }

            var response = new GenericResponse<IList<T>>();

            try
            {
                response = new GenericResponse<IList<T>>
                {
                    Result = await CallToConsumeWebService(url).ConfigureAwait(false)
                };
            }
            catch (Exception ex)
            {
                response.Error = ex;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<IList<T>> CallToConsumeWebService(string url)
        {
            var response = await ConsumeServiceGetAsync(url).ConfigureAwait(false);

            if (response == null)
            {
                return new List<T>(null!);
            }

            try
            {
                if (!string.IsNullOrEmpty(response))
                {
                    var apiResponse = JsonConvert.DeserializeObject<IList<T>>(response)!;

                    return apiResponse!;
                }
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }

            return null!;
        }

        public async Task<GenericResponse<T>> ExternalServiceUtilityByEntity(string url)
        {
            var response = new GenericResponse<T>
            {
                Result = await CallToConsumeWebServiceByEntity(url).ConfigureAwait(false)
            };

            return response;
        }

        private async Task<T> CallToConsumeWebServiceByEntity(string url)
        {
            var response = await ConsumeServiceGetAsync(url)!.ConfigureAwait(false);

            try
            {
                if (!string.IsNullOrEmpty(response))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(response);

                    return apiResponse!.Result!;
                }
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                Console.WriteLine(ex.Message);

                return null!;
            }

            return null!;
        }

        public async Task<GenericResponse<T>> ExternalServiceUtilityCreate(string url, HttpContent content)
        {
            var response = new GenericResponse<T>();

            response = await CallToConsumeWebServicePost(url, content).ConfigureAwait(false);

            return response;
        }

        private async Task<GenericResponse<T>> CallToConsumeWebServicePost(string url, HttpContent content)
        {
            var response = await ConsumeServicePostAsync(url, content).ConfigureAwait(false);

            var genericResponse = new GenericResponse<T>();

            if (response == null)
            {
                genericResponse.Message = "La consulta no devolvió resultados";

                return genericResponse;
            }

            try
            {
                if (!string.IsNullOrEmpty(response))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(response)!;

                    genericResponse = new GenericResponse<T>
                    {
                        Error = apiResponse.Error,
                        Result = apiResponse.Result,
                        Message = apiResponse.Message
                    };

                    return genericResponse!;
                }
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                Console.WriteLine(ex.Message);

                genericResponse.Error = ex.Message;

                return genericResponse!;
            }

            return null!;
        }

        public async Task<GenericResponse<T>> ExternalServiceUtilityByEntityById(string url)
        {
            var response = new GenericResponse<T>
            {
                Result = await CallToConsumeWebServiceByEntityById(url).ConfigureAwait(false)
            };

            return response;
        }

        private async Task<T> CallToConsumeWebServiceByEntityById(string url)
        {
            var response = await ConsumeServiceGetAsync(url)!.ConfigureAwait(false);

            try
            {
                if (!string.IsNullOrEmpty(response))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(response);

                    return apiResponse!.Result!;
                }
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                Console.WriteLine(ex.Message);

                return null!;
            }

            return null!;
        }

        public async Task<GenericResponse<T>> ExternalServiceUtilityUpdate(string url, HttpContent content)
        {
            var response = new GenericResponse<T>();

            response = await CallToConsumeWebServiceUpdate(url, content).ConfigureAwait(false);

            return response;
        }

        private async Task<GenericResponse<T>> CallToConsumeWebServiceUpdate(string url, HttpContent content)
        {
            var response = await ConsumeServiceUpdateAsync(url, content).ConfigureAwait(false);

            var genericResponse = new GenericResponse<T>();

            if (response is null)
            {
                genericResponse.Message = "La consulta no devolvió resultado";

                return genericResponse;
            }

            try
            {
                if (!string.IsNullOrEmpty(response))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(response)!;

                    genericResponse = new GenericResponse<T>
                    {
                        Error = apiResponse.Error,
                        Result = apiResponse.Result,
                        Message = apiResponse.Message
                    };

                    return genericResponse!;
                }
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                Console.WriteLine(ex.Message);

                genericResponse.Error = ex.Message;

                return genericResponse!;
            }

            return null!;
        }

        public class Objeto
        {
            public string Error { get; } = string.Empty;
        }

        public async Task<string> ConsumeServiceGetAsync(string url)
        {
            //using var httpClient = new HttpClient();
            using var httpClient = _httpClientFactory.CreateClient();

            ResiliencePipeline<HttpResponseMessage> pipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
                .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
                {
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>() //define las condiciones para hacer los reintentos
                        .Handle<Exception>()
                        .HandleResult(static result => !result.IsSuccessStatusCode), //reintentos para excepciones o respuestas no exitosa
                    Delay = TimeSpan.FromSeconds(2), //tiempo de espera entre reintentos
                    MaxRetryAttempts = 3,
                    BackoffType = DelayBackoffType.Constant //tipo de retraso entre reintentos
                })
                .Build();

            var response = await pipeline.ExecuteAsync(async token => await httpClient.GetAsync(url), CancellationToken.None);

            Console.WriteLine($"the response is... {await response.Content.ReadAsStringAsync()}");

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> ConsumeServicePostAsync(string url, HttpContent content)
        {
            using var httpClient = _httpClientFactory.CreateClient();

            using var response = await Policy.HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), (result, timeSpan, retryCount, context) => { })
                .ExecuteAsync(() => httpClient.PostAsync(url, content));

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public async Task<string> ConsumeServiceUpdateAsync(string url, HttpContent content)
        {
            using var httpClient = _httpClientFactory.CreateClient();

            using var response = await Policy.HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), (result, timeSpan, retryCount, context) => { })
                .ExecuteAsync(() => httpClient.PutAsync(url, content));

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public async Task<GenericResponse<T>> ExternalServiceUtilityDelete(string url)
        {
            var response = new GenericResponse<T>();

            response = await CallToConsumeWebServiceDelete(url).ConfigureAwait(false);

            return response;
        }

        private async Task<GenericResponse<T>> CallToConsumeWebServiceDelete(string url)
        {
            var response = await ConsumeServiceDeleteAsync(url)!.ConfigureAwait(false);

            var genericResponse = new GenericResponse<T>();

            try
            {
                if (!string.IsNullOrEmpty(response))
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(response);

                    genericResponse = new GenericResponse<T>
                    {
                        Error = apiResponse.Error,
                        IsSuccess = apiResponse.IsSuccess,
                        Message = apiResponse.Message,
                        Result = apiResponse.Result
                    };

                    return genericResponse;
                }
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                Console.WriteLine(ex.Message);

                return null!;
            }

            return null!;
        }

        public async Task<string> ConsumeServiceDeleteAsync(string url)
        {
            using var httpClient = _httpClientFactory.CreateClient();

            using var response = await Policy.HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), (result, timeSpan, retryCount, context) => { })
                .ExecuteAsync(() => httpClient.DeleteAsync(url));

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}
