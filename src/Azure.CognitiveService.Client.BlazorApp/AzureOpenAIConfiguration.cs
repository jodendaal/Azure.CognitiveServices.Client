namespace Azure.CognitiveService.Client.BlazorApp
{
    public class AzureOpenAIConfiguration
    {
        public Textcompletion TextCompletion { get; set; }
        public Embeddings Embeddings { get; set; }
        public Chat Chat { get; set; }
    }

    public class Textcompletion
    {
        public string DeploymentName { get; set; }
        public string ApiVersion { get; set; }
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
    }

    public class Embeddings
    {
        public string DeploymentName { get; set; }
        public string ApiVersion { get; set; }
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
    }

    public class Chat
    {
        public string DeploymentName { get; set; }
        public string ApiVersion { get; set; }
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
    }

}
