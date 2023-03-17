using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Azure.CognitiveServices.Client.OpenAI.Services.Interfaces;

namespace Azure.CognitiveServices.Client.OpenAI.ExtensionMethods
{
    public static class EmbeddingsServiceExtensionMethods
    {
        public static Task<OpenAIHttpOperationResult<EmbeddingsResponse, ErrorResponse>> Create(this IEmbeddingsService service, string input, AzureOpenAIConfig config)
        {
            var request = new EmbeddingsRequest(input.ToList());
            return service.Create(request, config);
        }

        public static Task<OpenAIHttpOperationResult<EmbeddingsResponse, ErrorResponse>> Create(this IEmbeddingsService service, IList<string> input, AzureOpenAIConfig config)
        {
            var request = new EmbeddingsRequest(input);
            return service.Create(request, config);
        }
    }
}
