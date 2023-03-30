using Microsoft.Extensions.Options;
using Azure.CognitiveServices.Client;
using Polly;
using Polly.Extensions.Http;
using Azure.CognitiveService.API.Models;
using Azure.CognitiveServices.Client.OpenAI.Models;

namespace Azure.CognitiveService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add Azure OpenAI
            builder.Services
               .AddAzureOpenAIHttpService(httpClientOptions => GetRetryPolicy())
               .AddAzureOpenAITextCompletion()
               .AddAzureOpenAITextEmbeddings()
               .AddAzureOpenAIChatCompletion();

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Add Azure OpenAI Config
            builder.Services.Configure<AzureOpenAIConfiguration>(builder.Configuration.GetSection("OpenAI"));

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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }
    }
}