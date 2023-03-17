using Azure.CognitiveService.Client.BlazorApp.Data;
using Azure.CognitiveServices.Client.OpenAI.ExtensionMethods;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace Azure.CognitiveService.Client.BlazorApp
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
               .AddAzureOpenAITextEmbeddings();

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

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

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