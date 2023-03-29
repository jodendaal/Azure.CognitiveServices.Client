using Azure.CognitiveServices.Client;
using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Azure.CognitiveServices.Client.OpenAI.Services;
using Azure.CognitiveServices.Client.Services.Interfaces;

public class ChatCompletionService : BaseOpenAIService, IChatCompletionService
{
    private readonly IHttpService _httpService;

    public ChatCompletionService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public Task<OpenAIHttpResult<ChatCompletionResponse, ErrorResponse>> CreateAsync(ChatCompletionRequest chatRequest, AzureOpenAIConfig azureOpenAIConfig)
    {
        return ErrorHandler(() =>
        {
            chatRequest.Validate();

            var request = CreateRequest(
            $"{azureOpenAIConfig.ApiUrl}/openai/deployments/{azureOpenAIConfig.DeploymentName}/chat/completions?api-version={azureOpenAIConfig.ApiVersion}",
                azureOpenAIConfig,
                chatRequest);

            return _httpService.SendRequest<ChatCompletionResponse, ErrorResponse>(request);
        });
    }

   

    public IAsyncEnumerable<OpenAIHttpResult<ChatStreamCompletionResponse, ErrorResponse>> CreateStream(ChatCompletionRequest completionRequest, AzureOpenAIConfig azureOpenAIConfig)
    {
        completionRequest.Stream = true;

        completionRequest.Validate();

        var request = CreateRequest(
            $"{azureOpenAIConfig.ApiUrl}/openai/deployments/{azureOpenAIConfig.DeploymentName}/chat/completions?api-version={azureOpenAIConfig.ApiVersion}",
            azureOpenAIConfig,
            completionRequest);


        return _httpService.SendRequestStream<ChatStreamCompletionResponse, ErrorResponse>(request);
    }

    //public async Task<Result<TextCompletionResponse>> GetTest(TextCompletionRequest completionRequest, AzureOpenAIConfig azureOpenAIConfig)
    //{
    //    completionRequest.Validate();

    //    var request = CreateRequest(
    //    $"{azureOpenAIConfig.ApiUrl}/openai/deployments/{azureOpenAIConfig.DeploymentName}/completions?api-version={azureOpenAIConfig.ApiVersion}",
    //        azureOpenAIConfig,
    //        completionRequest);

    //    return _httpService.SendRequest<TextCompletionResponse, ErrorResponse>(request);
    //}
}