using Azure.CognitiveServices.Client.OpenAI.Models;
using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;

public interface IChatCompletionService
{
    /// <summary>
    /// <inheritdoc cref="IChatCompletionService"/>
    /// </summary>
    Task<OpenAIHttpResult<ChatCompletionResponse, ErrorResponse>> CreateAsync(ChatCompletionRequest request, AzureOpenAIConfig azureOpenAIConfig);


    /// <summary>
    /// <inheritdoc cref="IChatCompletionService"/>
    /// </summary>
    IAsyncEnumerable<OpenAIHttpResult<ChatStreamCompletionResponse, ErrorResponse>> CreateStream(ChatCompletionRequest request, AzureOpenAIConfig azureOpenAIConfig);
}