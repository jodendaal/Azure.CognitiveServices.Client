using System.Net;
using System.Threading.Tasks;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Azure.CognitiveService.Client.FunctionsApp
{
    public class TextCompletionApi
    {
        private readonly ILogger<TextCompletionApi> _logger;
        private readonly ITextCompletionService _textCompletionService;
        private AzureOpenAIConfig _config;
        public TextCompletionApi(ILogger<TextCompletionApi> log,ITextCompletionService textCompletionService,IOptionsMonitor<AzureOpenAIConfig> configs)
        {
            _logger = log;
            _textCompletionService = textCompletionService;
            _config = configs.Get("textCompletion");
        }
        
        [FunctionName("TextCompletion")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType:"application/json",bodyType:typeof(TextCompletionRequestExternal),Required =true,Description ="The Reqeust Body")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(TextCompletionResponse), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] TextCompletionRequestExternal textCompletionRequest)
        {

            if(!IsValid(textCompletionRequest,out ICollection<ValidationResult>  validationResults))
            {
                return new UnprocessableEntityObjectResult($"{string.Join("\r\n", validationResults.Select(s => s.ErrorMessage))}");
            }

            var azureTextCompletionRequest = new TextCompletionRequest(textCompletionRequest.Prompt)
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


            var response = await _textCompletionService.Get(azureTextCompletionRequest, _config);

            if (response.IsSuccess)
            {
                return new OkObjectResult(response.Value);
            }
            else if(response.ErrorResponse != null)
            {
                return new BadRequestObjectResult(response.ErrorResponse);
            }
            else
            {
                _logger.LogError($"An error occured processing request {response.Exception?.Message} {response.Exception?.StackTrace}");
                return new BadRequestResult();
            }           
        }

        public bool IsValid(object o, out ICollection<ValidationResult> validationResults)
        {
            validationResults = new List<ValidationResult>();
            return Validator.TryValidateObject(o, new ValidationContext(o, null, null), validationResults, true);
        }
    }

    public class TextCompletionRequestExternal 
    {
        [Required]
        public IList<string> Prompt { get; set; }


        [Required]
        [Range(1, 4096)]
        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; } = 16;

        public double Temperature { get; set; } = 1;

        [JsonPropertyName("top_p")]
        public double TopP { get; set; } = 1;

        public int? N { get; set; } = 1;

        [JsonPropertyName("logprobs")]
        public int? LogProbs { get; set; }

        public bool? Echo { get; set; }

        public IList<string>? Stop { get; set; }

        [JsonPropertyName("frequency_penalty")]
        public double FrequencyPenalty { get; set; } = 0;

        [JsonPropertyName("best_of")]
        public int? BestOf { get; set; }

        [JsonPropertyName("logit_bias")]
        public Dictionary<string, double> LogitBias { get; set; }

     
    }
}

