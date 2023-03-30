using Azure.CognitiveServices.Client.OpenAI.Models;
using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;

namespace Azure.CognitiveServices.Client.OpenAI.ExtensionMethods
{
    public static class TextCompletionServiceExtensionMethods
    {
        public static Task<OpenAIHttpResult<TextCompletionResponse, ErrorResponse>> CreateAsync(this ITextCompletionService textCompletion, string prompt, AzureOpenAIConfig azureOpenAIConfig, Action<TextCompletionRequest>? options = null)
        {
            var request = new TextCompletionRequest(prompt);
            options?.Invoke(request);
            return textCompletion.CreateAsync(request, azureOpenAIConfig);
        }

        public static Task<OpenAIHttpResult<TextCompletionResponse, ErrorResponse>> CreateAsync(this ITextCompletionService textCompletion, IList<string> prompt, AzureOpenAIConfig azureOpenAIConfig, Action<TextCompletionRequest>? options = null)
        {
            var request = new TextCompletionRequest(prompt);
            options?.Invoke(request);
            return textCompletion.CreateAsync(request, azureOpenAIConfig);
        }


        public static IAsyncEnumerable<OpenAIHttpResult<TextCompletionResponse, ErrorResponse>> CreateStream(this ITextCompletionService textCompletion, string prompt, AzureOpenAIConfig azureOpenAIConfig, Action<TextCompletionRequest>? options = null)
        {
            var request = new TextCompletionRequest(prompt);
            options?.Invoke(request);
            return textCompletion.CreateStream(request, azureOpenAIConfig);
        }


        public static IAsyncEnumerable<OpenAIHttpResult<TextCompletionResponse, ErrorResponse>> CreateStream(this ITextCompletionService textCompletion, IList<string> prompt, AzureOpenAIConfig azureOpenAIConfig, Action<TextCompletionRequest>? options = null)
        {
            var request = new TextCompletionRequest(prompt);
            options?.Invoke(request);
            return textCompletion.CreateStream(request, azureOpenAIConfig);
        }
    }
}
