using Azure.CognitiveServices.Client.OpenAI.Models;
using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Azure.CognitiveServices.Client.OpenAI.Services.Interfaces;

namespace Azure.CognitiveServices.Client.OpenAI.Services
{
    public class EmbeddingsService : BaseOpenAIService, IEmbeddingsService
    {
        private readonly IOpenAIHttpService _httpService;

        public EmbeddingsService(IOpenAIHttpService httpService)
        {
            _httpService = httpService;
        }

        public Task<OpenAIHttpResult<EmbeddingsResponse, ErrorResponse>> Create(EmbeddingsRequest model, AzureOpenAIConfig azureOpenAIConfig)
        {
            return ErrorHandler(() =>
            {
                model.Validate();

                var request = CreateRequest(
                    $"{azureOpenAIConfig.ApiUrl}/openai/deployments/{azureOpenAIConfig.DeploymentName}/embeddings?api-version={azureOpenAIConfig.ApiVersion}",
                    azureOpenAIConfig,
                    model);
                
                    return _httpService.SendRequest<EmbeddingsResponse, ErrorResponse>(request);
            });
        }
    }
}
