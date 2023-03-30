using Azure.CognitiveServices.Client;
using Azure.CognitiveServices.Client.OpenAI.Models;
using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Azure.CognitiveServices.Client.OpenAI.Services;
using Azure.CognitiveServices.Client.OpenAI.Services.Interfaces;

public class ChatCompletionService : BaseOpenAIService, IChatCompletionService
{
    private readonly IOpenAIHttpService _httpService;

    public ChatCompletionService(IOpenAIHttpService httpService)
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
}