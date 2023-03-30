using System.Text.Json.Serialization;

namespace Azure.CognitiveService.API.Models
{
    public class ChatCompletionRequestExternal
    {
        public IList<MessageExternal> Messages { get; set; }

        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; } = 16;

        public double Temperature { get; set; } = 1;

        [JsonPropertyName("top_p")]
        public double TopP { get; set; } = 1;
     
        public int? N { get; set; } = 1;

        public IList<string>? Stop { get; set; }

        [JsonPropertyName("presence_penalty")]
        public double PresencePenalty { get; set; } = 0;

        [JsonPropertyName("frequency_penalty")]
        public double FrequencyPenalty { get; set; } = 0;

        [JsonPropertyName("logit_bias")]
        public Dictionary<string, int>? LogitBias { get; set; } = null;

    }

    public class MessageExternal
    {
        
        public string Role { get; init; }
        public string Content { get; init; }
    }


}
