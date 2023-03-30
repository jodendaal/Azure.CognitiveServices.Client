using Azure.CognitiveService.API.Models;
using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace Azure.CognitiveService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TextCompletionStreamController : ControllerBase
    {
        private readonly ILogger<TextCompletionStreamController> _logger;
        private readonly ITextCompletionService _textCompletionService;
        private readonly AzureOpenAIConfig _config;
        public TextCompletionStreamController(
            ILogger<TextCompletionStreamController> logger,
            ITextCompletionService textCompletionService,
            IOptionsSnapshot<AzureOpenAIConfig> openAIConfig
            )
        {
            _logger = logger;
            _textCompletionService = textCompletionService;
            _config = openAIConfig.Get("textCompletion");
        }

        [HttpPost(Name = "CreateTextCompletionStream")]
        public async IAsyncEnumerable<TextCompletionResponse> CreateStream(TextCompletionRequestExternal request)
        {
            await foreach (var result in _textCompletionService.CreateStream(Convert(request), _config))
            {
                if(result.Value != null)
                {
                    yield return result.Value;
                }
            }
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