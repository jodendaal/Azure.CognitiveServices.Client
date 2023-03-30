using Azure.CognitiveService.Client.FunctionsApp;
using Azure.CognitiveService.Client.FunctionsApp.Models;
using Azure.CognitiveServices.Client;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
[assembly: FunctionsStartup(typeof(Startup))]
namespace Azure.CognitiveService.Client.FunctionsApp
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder
                .AddUserSecrets<Startup>()
                .AddEnvironmentVariables();
            base.ConfigureAppConfiguration(builder);
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                  .AddAzureOpenAIHttpService()
                  .AddAzureOpenAITextCompletion()
                  .AddAzureOpenAITextEmbeddings();

            var configuration = builder.GetContext().Configuration;

            builder.Services.Configure<AzureOpenAIConfiguration>(configuration.GetSection("OpenAI"));

            builder.Services.AddOptions<AzureOpenAIConfig>("textCompletion").Configure<IOptions<AzureOpenAIConfiguration>>((o, e) =>
            {
                o.ApiVersion = e.Value.TextCompletion.ApiVersion;
                o.ApiKey = e.Value.TextCompletion.ApiKey;
                o.ApiUrl = e.Value.TextCompletion.ApiUrl;
                o.DeploymentName = e.Value.TextCompletion.DeploymentName;
            });

            builder.Services.AddOptions<AzureOpenAIConfig>("textEmbeddings").Configure<IOptions<AzureOpenAIConfiguration>>((o, e) =>
            {
                o.ApiVersion = e.Value.Embeddings.ApiVersion;
                o.ApiKey = e.Value.Embeddings.ApiKey;
                o.ApiUrl = e.Value.Embeddings.ApiUrl;
                o.DeploymentName = e.Value.Embeddings.DeploymentName;
            });


            builder.Services.AddOptions<AzureOpenAIConfig>("chat").Configure<IOptions<AzureOpenAIConfiguration>>((o, e) =>
            {
                o.ApiVersion = e.Value.Chat.ApiVersion;
                o.ApiKey = e.Value.Chat.ApiKey;
                o.ApiUrl = e.Value.Chat.ApiUrl;
                o.DeploymentName = e.Value.Chat.DeploymentName;
            });
        }
    }
}
