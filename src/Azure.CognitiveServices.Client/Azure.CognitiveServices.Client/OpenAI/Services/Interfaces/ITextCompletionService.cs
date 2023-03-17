using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;

public interface ITextCompletionService
{
    /// <summary>
    /// <inheritdoc cref="ITextCompletionService"/>
    /// </summary>
    Task<OpenAIHttpOperationResult<TextCompletionResponse, ErrorResponse>> Get(TextCompletionRequest request, AzureOpenAIConfig azureOpenAIConfig);

    /// <summary>
    /// <inheritdoc cref="ITextCompletionService"/>
    /// </summary>
    IAsyncEnumerable<OpenAIHttpOperationResult<TextCompletionResponse, ErrorResponse>> GetStream(TextCompletionRequest request, AzureOpenAIConfig azureOpenAIConfig);
}