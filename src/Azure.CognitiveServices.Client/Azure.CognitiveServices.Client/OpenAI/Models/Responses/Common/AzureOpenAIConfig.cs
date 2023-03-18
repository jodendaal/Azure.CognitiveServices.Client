namespace Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common
{
    public record AzureOpenAIConfig
    {
        public string DeploymentName { get; set; }
        public string ApiVersion { get; set; } = "2022-12-01";
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public string AccessToken { get; set; }
    }
}
