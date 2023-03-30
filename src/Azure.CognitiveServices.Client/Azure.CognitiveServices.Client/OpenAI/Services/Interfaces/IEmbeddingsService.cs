using Azure.CognitiveServices.Client.OpenAI.Models;
using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;

namespace Azure.CognitiveServices.Client.OpenAI.Services.Interfaces
{
    public interface IEmbeddingsService
    {
        Task<OpenAIHttpResult<EmbeddingsResponse, ErrorResponse>> Create(EmbeddingsRequest model, AzureOpenAIConfig azureOpenAIConfig);
    }
}
