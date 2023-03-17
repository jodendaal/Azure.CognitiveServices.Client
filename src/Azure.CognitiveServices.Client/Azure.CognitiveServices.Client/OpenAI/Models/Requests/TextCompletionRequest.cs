using System.Text.Json.Serialization;

namespace Azure.CognitiveServices.Client.OpenAI.Models.Requests
{
    public class TextCompletionRequest
    {
        public TextCompletionRequest(IList<string> prompt)
        {
            Prompt = prompt;
        }
        public TextCompletionRequest(string prompt) : this(prompt.ToList()) { }

        public IList<string> Prompt { get; set; }


        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; } = 16;

        public double Temperature { get; set; } = 1;

        [JsonPropertyName("top_p")]
        public double TopP { get; set; } = 1;

        public int? N { get; set; } = 1;

        public bool Stream { get; internal set; }

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
