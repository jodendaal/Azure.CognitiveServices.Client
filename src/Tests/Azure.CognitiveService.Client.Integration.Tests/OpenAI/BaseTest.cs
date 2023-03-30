using Azure.CognitiveServices.Client;
using Azure.CognitiveServices.Client.OpenAI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Azure.CognitiveService.Client.Integration.Tests.OpenAI
{
    public class BaseTest
    {
        
        public IServiceProvider ServiceProvider;

        public BaseTest()
        {
            var host = Host.CreateDefaultBuilder()
          .ConfigureAppConfiguration(config =>
          {
              config
                .AddJsonFile("appSettings.json")
                .AddUserSecrets<BaseTest>();
          })
          .ConfigureServices((builder, services) =>
          {
              services
               .AddAzureOpenAIHttpService()
               .AddAzureOpenAITextCompletion()
               .AddAzureOpenAITextEmbeddings()
               .AddAzureOpenAIChatCompletion();


              services.Configure<AzureOpenAIConfiguration>(builder.Configuration.GetSection("OpenAI"));

              services.AddOptions<AzureOpenAIConfig>("textCompletion").Configure<IOptions<AzureOpenAIConfiguration>>((o, e) =>
              {
                  o.ApiVersion = e.Value.TextCompletion.ApiVersion;
                  o.ApiKey = e.Value.TextCompletion.ApiKey;
                  o.ApiUrl = e.Value.TextCompletion.ApiUrl;
                  o.DeploymentName = e.Value.TextCompletion.DeploymentName;
              });

              services.AddOptions<AzureOpenAIConfig>("textEmbeddings").Configure<IOptions<AzureOpenAIConfiguration>>((o, e) =>
              {
                  o.ApiVersion = e.Value.Embeddings.ApiVersion;
                  o.ApiKey = e.Value.Embeddings.ApiKey;
                  o.ApiUrl = e.Value.Embeddings.ApiUrl;
                  o.DeploymentName = e.Value.Embeddings.DeploymentName;
              });

              services.AddOptions<AzureOpenAIConfig>("chat").Configure<IOptions<AzureOpenAIConfiguration>>((o, e) =>
              {
                  o.ApiVersion = e.Value.Chat.ApiVersion;
                  o.ApiKey = e.Value.Chat.ApiKey;
                  o.ApiUrl = e.Value.Chat.ApiUrl;
                  o.DeploymentName = e.Value.Chat.DeploymentName;
              });

          }).Build();

            ServiceProvider = host.Services;
        }
    }
}
