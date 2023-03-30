using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Message = Azure.CognitiveServices.Client.OpenAI.Models.Requests.Message;
using Azure.CognitiveServices.Client.OpenAI.Models;

namespace Azure.CognitiveServices.Client.OpenAI.ExtensionMethods
{
    public static class ChatCompletionServiceExtensionMethods
    {
        /// <summary>
        /// <inheritdoc cref="IChatCompletionService"/>
        /// </summary>
        public static Task<OpenAIHttpResult<ChatCompletionResponse, ErrorResponse>> Get(this IChatCompletionService chatCompletion, string userMessage, AzureOpenAIConfig azureOpenAIConfig, Action<ChatCompletionRequest>? options = null)
        {
            var request = new ChatCompletionRequest(Message.Create(ChatRoleType.User, userMessage));
            options?.Invoke(request);
            return chatCompletion.CreateAsync(request, azureOpenAIConfig);
        }

        /// <summary>
        /// <inheritdoc cref="IChatCompletionService"/>
        /// </summary>
        public static Task<OpenAIHttpResult<ChatCompletionResponse, ErrorResponse>> Get(this IChatCompletionService chatCompletion, IList<Message> messages,AzureOpenAIConfig azureOpenAIConfig, Action<ChatCompletionRequest>? options = null)
        {
            var request = new ChatCompletionRequest(messages);
            options?.Invoke(request);
            return chatCompletion.CreateAsync(request, azureOpenAIConfig);
        }


        /// <summary>
        /// <inheritdoc cref="IChatCompletionService"/>
        /// </summary>
        public static Task<OpenAIHttpResult<ChatCompletionResponse, ErrorResponse>> Get(this IChatCompletionService chatCompletion, Message message, AzureOpenAIConfig azureOpenAIConfig, Action<ChatCompletionRequest>? options = null)
        {
            var request = new ChatCompletionRequest(message);
            options?.Invoke(request);
            return chatCompletion.CreateAsync(request, azureOpenAIConfig);
        }


        /// <summary>
        /// <inheritdoc cref="IChatCompletionService"/>
        /// </summary>
        public static IAsyncEnumerable<OpenAIHttpResult<ChatStreamCompletionResponse, ErrorResponse>> CreateStream(this IChatCompletionService chatCompletion, string userMessage,  AzureOpenAIConfig azureOpenAIConfig, Action<ChatCompletionRequest>? options = null)
        {
            var request = new ChatCompletionRequest(Message.Create(ChatRoleType.User, userMessage));
            options?.Invoke(request);
            return chatCompletion.CreateStream(request, azureOpenAIConfig);
        }

        /// <summary>
        /// <inheritdoc cref="IChatCompletionService"/>
        /// </summary>
        public static IAsyncEnumerable<OpenAIHttpResult<ChatStreamCompletionResponse, ErrorResponse>> CreateStream(this IChatCompletionService chatCompletion, IList<Message> messages,  AzureOpenAIConfig azureOpenAIConfig, Action<ChatCompletionRequest>? options = null)
        {
            var request = new ChatCompletionRequest(messages);
            options?.Invoke(request);
            return chatCompletion.CreateStream(request, azureOpenAIConfig);
        }


        /// <summary>
        /// <inheritdoc cref="IChatCompletionService"/>
        /// </summary>
        public static IAsyncEnumerable<OpenAIHttpResult<ChatStreamCompletionResponse, ErrorResponse>> CreateStream(this IChatCompletionService chatCompletion, Message message,AzureOpenAIConfig azureOpenAIConfig, Action<ChatCompletionRequest>? options = null)
        {
            var request = new ChatCompletionRequest(message);
            options?.Invoke(request);
            return chatCompletion.CreateStream(request, azureOpenAIConfig);
        }
    }
}
