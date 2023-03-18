namespace Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common
{
    public class ErrorResponse
    {
        public Error Error { get; set; }
    }

    public class Error
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public object Param { get; set; }
        public string Code { get; set; }
    }
}
