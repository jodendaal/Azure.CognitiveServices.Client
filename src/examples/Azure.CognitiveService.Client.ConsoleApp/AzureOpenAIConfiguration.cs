namespace Azure.CognitiveService.Client.ConsoleApp
{
    public class AzureOpenAIConfiguration
    {
        public AzureOpenAIEndPointConfiguration TextCompletion { get; set; }
        public AzureOpenAIEndPointConfiguration Embeddings { get; set; }
        public AzureOpenAIEndPointConfiguration Chat { get; set; }
    }

    public class AzureOpenAIEndPointConfiguration
    {
        public string DeploymentName { get; set; }
        public string ApiVersion { get; set; }
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
    }
}
