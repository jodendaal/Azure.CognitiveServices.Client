using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using Azure.CognitiveServices.Client.OpenAI.ExtensionMethods;
using Microsoft.Extensions.DependencyInjection;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Microsoft.Extensions.Options;
using Azure.CognitiveServices.Client.OpenAI.Services.Interfaces;

namespace Azure.CognitiveService.Client.ConsoleApp
{
    internal class Program
    {
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }

        static async Task Main(string[] args)
        {
            using var host = Host.CreateDefaultBuilder(args)
           .ConfigureAppConfiguration(config =>
           {
;              config
               .AddJsonFile("appSettings.json")
               .AddUserSecrets(typeof(Program).Assembly);


           }).ConfigureServices((builder, services) =>
           {
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

               services
               .AddAzureOpenAIHttpService(httpClientOptions => GetRetryPolicy())
               .AddAzureOpenAITextCompletion()
               .AddAzureOpenAITextEmbeddings();
           })
            .Build();

            var config = host.Services.GetRequiredService<IOptionsSnapshot<AzureOpenAIConfig>>();
            var textCompletionConfig = config.Get("textCompletion");
            var textEmbeddingsConfig = config.Get("textEmbeddings");


            //Call Text Completion API Synchronise
            var textCompletionService = host.Services.GetRequiredService<ITextCompletionService>()!;
            var textCompletionResponse = await textCompletionService.Get("Say This is a test.", textCompletionConfig, options => {
                options.N = 1;
                options.TopP = 1;
            });

            Console.WriteLine(textCompletionResponse.Result!.Choices[0].Text);


            //Call Text Completion API Stream
            await foreach (var response in textCompletionService.GetStream("Say This is a test.", textCompletionConfig, options =>
            {
                options.N = 1;
                options.TopP = 1;
            }))
            {
                Console.WriteLine(response.Result!.Choices[0].Text);
            }

            //Call TextEmbeddings API
            var textEmbeddingsService = host.Services.GetRequiredService<IEmbeddingsService>()!;
            var textEmbeddingsResponse = await textEmbeddingsService.Create("Say This is a test.", textEmbeddingsConfig);
            Console.WriteLine(textEmbeddingsResponse.Result!.Data);
        }
    }
}