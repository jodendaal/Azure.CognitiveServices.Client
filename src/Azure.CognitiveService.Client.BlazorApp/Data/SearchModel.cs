using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using System.ComponentModel.DataAnnotations;

namespace Azure.CognitiveService.Client.BlazorApp.Data
{
    public class SearchModel
    {
        [Required]
        public string SearchText { get; set; }

        [Required]
        public int NoOfResults { get; set; } = 2;

        [Required]
        public int MaxTokens { get; set; } = 200;
    }

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

        public ChatCompletionRequest ChatCompletionRequest { get; set; } = new ChatCompletionRequest(new List<Message>());
    }
}
