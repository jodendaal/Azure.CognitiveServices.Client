using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Azure.CognitiveService.Client.FunctionsApp.Models
{
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

