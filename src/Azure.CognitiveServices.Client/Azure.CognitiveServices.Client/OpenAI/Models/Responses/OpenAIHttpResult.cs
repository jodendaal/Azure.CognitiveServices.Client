using Azure.CognitiveServices.Client.Models.OperationResult;
using System.Net;
using System.Text.Json;

namespace Azure.CognitiveServices.Client.OpenAI.Models.Responses
{
    public class OpenAIHttpResult<T, TError> : HttpResult<T>
    {
        public OpenAIHttpResult(T? result, HttpStatusCode httpStatusCode) : base(result, httpStatusCode)
        {
        }

        public OpenAIHttpResult(Exception exception, HttpStatusCode httpStatusCode, string? errorMessaage = null) : base(exception, httpStatusCode, errorMessaage)
        {
            if (!string.IsNullOrWhiteSpace(errorMessaage))
            {
                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                ErrorResponse = JsonSerializer.Deserialize<TError>(errorMessaage, serializeOptions);
            }
        }

        public TError? ErrorResponse { get; internal set; }

        public static implicit operator OpenAIHttpResult<T, TError>(T? result) => new(result, HttpStatusCode.OK);
        public static implicit operator T(OpenAIHttpResult<T, TError> result) => result.Value!;
    }
}
