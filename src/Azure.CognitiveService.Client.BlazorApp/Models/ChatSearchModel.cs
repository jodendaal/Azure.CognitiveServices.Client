using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using System.ComponentModel.DataAnnotations;

namespace Azure.CognitiveService.Client.BlazorApp.Models
{
    public class ChatSearchModel
    {
        public ChatSearchModel()
        {
            Id = Guid.NewGuid().ToString();

        }
        public string Id { get;  set; }
        public string Name { get; set; }

        [Required]
        public string SearchText { get; set; }

        public string System { get; set; } = "";

        public string Assistant { get; set; } = "";

        public ChatCompletionRequest ChatCompletionRequest { get; set; } = new ChatCompletionRequest(new List<Message>()) {

            MaxTokens = 2048,
            Temperature = 0.7,
            TopP = 0.95
        };

        public int NumberOfPreviousMessageToInclude { get; set; } = 10;
    }
}
