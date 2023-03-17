using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Azure.CognitiveServices.Client.OpenAI.Services;
using Azure.CognitiveServices.Client.Services.Interfaces;

public class TextCompletionService : BaseOpenAIService,ITextCompletionService
{
    private readonly IHttpService _httpService;

    public TextCompletionService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public Task<OpenAIHttpOperationResult<TextCompletionResponse, ErrorResponse>> Get(TextCompletionRequest completionRequest, AzureOpenAIConfig azureOpenAIConfig)
    {
        return ErrorHandler(() =>
        {
            var request = CreateRequest(
            $"{azureOpenAIConfig.ApiUrl}/openai/deployments/{azureOpenAIConfig.DeploymentName}/completions?api-version={azureOpenAIConfig.ApiVersion}",
                azureOpenAIConfig,
                completionRequest);

            return _httpService.SendRequest<TextCompletionResponse, ErrorResponse>(request);
        });
    }

    public IAsyncEnumerable<OpenAIHttpOperationResult<TextCompletionResponse, ErrorResponse>> GetStream(TextCompletionRequest completionRequest, AzureOpenAIConfig azureOpenAIConfig)
    {
        completionRequest.Stream = true;
       
        var request = CreateRequest(
        $"{azureOpenAIConfig.ApiUrl}/openai/deployments/{azureOpenAIConfig.DeploymentName}/completions?api-version={azureOpenAIConfig.ApiVersion}",
            azureOpenAIConfig,
            completionRequest);

        return _httpService.SendRequestStream<TextCompletionResponse, ErrorResponse>(request);
    }
}