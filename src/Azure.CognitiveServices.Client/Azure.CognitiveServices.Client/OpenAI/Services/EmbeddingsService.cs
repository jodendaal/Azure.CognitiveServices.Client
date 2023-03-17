using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Azure.CognitiveServices.Client.OpenAI.Services.Interfaces;
using Azure.CognitiveServices.Client.Services.Interfaces;

namespace Azure.CognitiveServices.Client.OpenAI.Services
{
    public class EmbeddingsService : BaseOpenAIService, IEmbeddingsService
    {
        private readonly IHttpService _httpService;

        public EmbeddingsService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public Task<OpenAIHttpOperationResult<EmbeddingsResponse, ErrorResponse>> Create(EmbeddingsRequest model, AzureOpenAIConfig azureOpenAIConfig)
        {
            return ErrorHandler(() =>
            {
                var request = CreateRequest(
                   $"{azureOpenAIConfig.ApiUrl}/openai/deployments/{azureOpenAIConfig.DeploymentName}/embeddings?api-version={azureOpenAIConfig.ApiVersion}",
                   azureOpenAIConfig,
                   model);

                return _httpService.SendRequest<EmbeddingsResponse, ErrorResponse>(request);
            });
        }
    }
}
