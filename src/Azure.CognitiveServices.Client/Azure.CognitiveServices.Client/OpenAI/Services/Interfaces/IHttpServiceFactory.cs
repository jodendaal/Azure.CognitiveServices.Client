using Azure.CognitiveServices.Client.OpenAI.Models;

namespace Azure.CognitiveServices.Client.OpenAI.Services.Interfaces
{
    public interface IOpenAIHttpServiceFactory
    {
        HttpClient CreateClient(AzureOpenAIConfig config);
    }

    public class HttpServiceFactory : IOpenAIHttpServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpServiceFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient CreateClient(AzureOpenAIConfig config)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(config.ApiUrl);

            if (!string.IsNullOrWhiteSpace(config.ApiKey))
            {
                httpClient.DefaultRequestHeaders.Add("api-key", config.ApiKey);
            }

            if (!string.IsNullOrWhiteSpace(config.AccessToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", config.AccessToken);
            }

            return httpClient;
        }
    }
}
