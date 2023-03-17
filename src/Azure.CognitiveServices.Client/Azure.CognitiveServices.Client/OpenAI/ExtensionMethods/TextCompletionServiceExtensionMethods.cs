using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;

namespace Azure.CognitiveServices.Client.OpenAI.ExtensionMethods
{
    public static class TextCompletionServiceExtensionMethods
    {
        public static Task<OpenAIHttpOperationResult<TextCompletionResponse, ErrorResponse>> Get(this ITextCompletionService textCompletion, string prompt, AzureOpenAIConfig azureOpenAIConfig, Action<TextCompletionRequest>? options = null)
        {
            var request = new TextCompletionRequest(prompt);
            options?.Invoke(request);
            return textCompletion.Get(request, azureOpenAIConfig);
        }

        public static Task<OpenAIHttpOperationResult<TextCompletionResponse, ErrorResponse>> Get(this ITextCompletionService textCompletion, IList<string> prompt, AzureOpenAIConfig azureOpenAIConfig, Action<TextCompletionRequest>? options = null)
        {
            var request = new TextCompletionRequest(prompt);
            options?.Invoke(request);
            return textCompletion.Get(request, azureOpenAIConfig);
        }


        public static IAsyncEnumerable<OpenAIHttpOperationResult<TextCompletionResponse, ErrorResponse>> GetStream(this ITextCompletionService textCompletion, string prompt, AzureOpenAIConfig azureOpenAIConfig, Action<TextCompletionRequest>? options = null)
        {
            var request = new TextCompletionRequest(prompt);
            options?.Invoke(request);
            return textCompletion.GetStream(request, azureOpenAIConfig);
        }


        public static IAsyncEnumerable<OpenAIHttpOperationResult<TextCompletionResponse, ErrorResponse>> GetStream(this ITextCompletionService textCompletion, IList<string> prompt, AzureOpenAIConfig azureOpenAIConfig, Action<TextCompletionRequest>? options = null)
        {
            var request = new TextCompletionRequest(prompt);
            options?.Invoke(request);
            return textCompletion.GetStream(request, azureOpenAIConfig);
        }
    }
}
