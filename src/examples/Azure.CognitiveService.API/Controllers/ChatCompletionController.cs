using Azure.CognitiveService.API.Models;
using Azure.CognitiveServices.Client.OpenAI.Models.Exceptions;
using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Azure.CognitiveService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatCompletionController : ControllerBase
    {
        private readonly ILogger<ChatCompletionController> _logger;
        private readonly IChatCompletionService _chatCompletionService;
        private readonly AzureOpenAIConfig _config;
        public ChatCompletionController(
            ILogger<ChatCompletionController> logger,
            IChatCompletionService chatCompletionService,
            IOptionsSnapshot<AzureOpenAIConfig> openAIConfig
            )
        {
            _logger = logger;
            _chatCompletionService = chatCompletionService;
            _config = openAIConfig.Get("chat");
        }

        [HttpPost(Name = "CreateChatCompletion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType(typeof(ChatCompletionResponse))]
        public async Task<IActionResult> CreateChatCompletion(ChatCompletionRequestExternal request)
        {
            try
            {
                var response = await _chatCompletionService.CreateAsync(Convert(request), _config);

                if (response.IsSuccess)
                {
                    return Ok(response.Value);
                }

                return BadRequest(response.ErrorResponse?.Error);
            }
            catch(OpenAIValidationException validationException)
            {
                return BadRequest(validationException.Message);
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