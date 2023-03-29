using Azure.CognitiveServices.Client.OpenAI.Models.Requests;

namespace Azure.CognitiveServices.Client.OpenAI.ExtensionMethods
{
    public static class MessageExtensions
    {
        public static IList<Message> ToList(this Message value)
        {
            return new List<Message> { value };
        }
    }
}
