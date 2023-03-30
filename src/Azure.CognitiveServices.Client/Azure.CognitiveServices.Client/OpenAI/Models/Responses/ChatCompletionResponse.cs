using System.Text.Json.Serialization;

namespace Azure.CognitiveServices.Client.OpenAI.Models.Responses
{
    public class ChatCompletionBaseResponse
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public int Created { get; set; }
        public string Model { get; set; }
     
    }

    public class ChatCompletionResponse : ChatCompletionBaseResponse
    {
        public ChatChoice[] Choices { get; set; }
    }

    public class ChatStreamCompletionResponse : ChatCompletionBaseResponse
    {
        public ChatStreamChoice[] Choices { get; set; }
    }

    public class ChatChoice
    {
        public ChatResponseMessage Message { get; set; }
        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }
        public int Index { get; set; }
    }

    public class ChatResponseMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }


    public class ChatStreamChoice
    {
        public Delta Delta { get; set; }
        public int Index { get; set; }
        [JsonPropertyName("finish_reason")]
        public object FinishReason { get; set; }
    }

    public class Delta
    {
        public string Content { get; set; }
    }
}
