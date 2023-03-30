using Azure.CognitiveService.API.Models;
using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Azure.CognitiveService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatCompletionStreamController : ControllerBase
    {
        private readonly ILogger<ChatCompletionStreamController> _logger;
        private readonly IChatCompletionService _chatCompletionService;
        private readonly AzureOpenAIConfig _config;
        public ChatCompletionStreamController(
            ILogger<ChatCompletionStreamController> logger,
            IChatCompletionService chatCompletionService,
            IOptionsSnapshot<AzureOpenAIConfig> openAIConfig
            )
        {
            _logger = logger;
            _chatCompletionService = chatCompletionService;
            _config = openAIConfig.Get("chat");
        }

        [HttpPost(Name = "CreateChatCompletionStream")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType(typeof(ChatStreamCompletionResponse))]
        public async IAsyncEnumerable<ChatStreamCompletionResponse> CreateChatCompletionStream(ChatCompletionRequestExternal request)
        {
            await foreach (var result in _chatCompletionService.CreateStream(Convert(request), _config))
            {
                if (result.Value != null)
                {
                    yield return result.Value;
                }
            }
        }

        private ChatCompletionRequest Convert(ChatCompletionRequestExternal textCompletionRequest)
        {
            var messages = textCompletionRequest.Messages.Select(message => Message.Create(message.Role, message.Content)).ToList();

            return new ChatCompletionRequest(messages)
            {
                Temperature = textCompletionRequest.Temperature,
                FrequencyPenalty = textCompletionRequest.FrequencyPenalty,
                LogitBias = textCompletionRequest?.LogitBias,
                MaxTokens = textCompletionRequest.MaxTokens,
                N = textCompletionRequest.N,
                Stop = textCompletionRequest?.Stop,
                TopP = textCompletionRequest.TopP,
                PresencePenalty = textCompletionRequest.PresencePenalty
            };
        }
    }
}