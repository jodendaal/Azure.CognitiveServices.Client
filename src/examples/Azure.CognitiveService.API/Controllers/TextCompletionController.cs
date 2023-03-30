using Azure.CognitiveService.API.Models;
using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Azure.CognitiveService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TextCompletionController : ControllerBase
    {
        private readonly ILogger<TextCompletionController> _logger;
        private readonly ITextCompletionService _textCompletionService;
        private readonly AzureOpenAIConfig _config;
        public TextCompletionController(
            ILogger<TextCompletionController> logger,
            ITextCompletionService textCompletionService,
            IOptionsSnapshot<AzureOpenAIConfig> openAIConfig
            )
        {
            _logger = logger;
            _textCompletionService = textCompletionService;
            _config = openAIConfig.Get("textCompletion");
        }

        [HttpPost(Name = "CreateTextCompletion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType(typeof(TextCompletionResponse))]
        public async Task<IActionResult> CreateAsync(TextCompletionRequestExternal request)
        {
            var response = await _textCompletionService.CreateAsync(Convert(request), _config);

            if(response.IsSuccess)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.ErrorResponse?.Error);
        }


        private TextCompletionRequest Convert(TextCompletionRequestExternal textCompletionRequest)
        {
            return new TextCompletionRequest(textCompletionRequest.Prompt)
            {
                Temperature = textCompletionRequest.Temperature,
                BestOf = textCompletionRequest?.BestOf,
                Echo = textCompletionRequest?.Echo,
                FrequencyPenalty = textCompletionRequest.FrequencyPenalty,
                LogitBias = textCompletionRequest?.LogitBias,
                LogProbs = textCompletionRequest?.LogProbs,
                MaxTokens = textCompletionRequest.MaxTokens,
                N = textCompletionRequest.N,
                Stop = textCompletionRequest?.Stop,
                TopP = textCompletionRequest.TopP
            };
        }
    }
}