using Azure.CognitiveServices.Client.OpenAI.Models;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using System.Text;
using System.Text.Json;

namespace Azure.CognitiveServices.Client.OpenAI.Services
{
    public class BaseOpenAIService
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        internal HttpRequestMessage CreateRequest(string uri, AzureOpenAIConfig config, object data)
        {
            HttpRequestMessage message = new(HttpMethod.Post, uri);

            if (!string.IsNullOrWhiteSpace(config.ApiKey))
            {
                message.Headers.Add("api-key", config.ApiKey);
            }

            if (!string.IsNullOrWhiteSpace(config.AccessToken))
            {
                message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", config.AccessToken);
            }
            message.Content = new StringContent(JsonSerializer.Serialize(data, _jsonSerializerOptions), Encoding.UTF8, "application/json");
            return message;
        }

        internal async Task<OpenAIHttpResult<T, TError>> ErrorHandler<T, TError>(Func<Task<OpenAIHttpResult<T, TError>>> func)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                return new OpenAIHttpResult<T, TError>(ex, System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
