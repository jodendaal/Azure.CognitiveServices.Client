using System.ComponentModel.DataAnnotations;

namespace Azure.CognitiveServices.Client.OpenAI.Models.Requests
{
    public class EmbeddingsRequest
    {
        public EmbeddingsRequest(IList<string> input)
        {
            Input = input;
        }

        public EmbeddingsRequest(string input) : this(input.ToList()) { }

        [Required]
        public IList<string> Input { get; set; } = new List<string>();
    }
}
