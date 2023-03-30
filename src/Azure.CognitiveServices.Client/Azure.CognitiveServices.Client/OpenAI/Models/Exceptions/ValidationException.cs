namespace Azure.CognitiveServices.Client.OpenAI.Models.Exceptions
{
    public class OpenAIValidationException : Exception
    {
        public OpenAIValidationException(string message): base(message)
        {
        }
    }
}
