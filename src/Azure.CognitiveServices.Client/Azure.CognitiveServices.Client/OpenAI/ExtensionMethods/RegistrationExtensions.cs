using Azure.CognitiveServices.Client.OpenAI.Services;
using Azure.CognitiveServices.Client.OpenAI.Services.Interfaces;
using Azure.CognitiveServices.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Azure.CognitiveServices.Client
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection AddAzureOpenAIHttpService(this IServiceCollection services, Action<IHttpClientBuilder> httpClientOptions = default!)
        {
            ConfigureHttpClientBuilder(services.AddHttpClient<IOpenAIHttpService, OpenAIHttpService>(), httpClientOptions);
            return services;
        }

        public static IServiceCollection AddAzureOpenAITextCompletion(this IServiceCollection services)
        {
            services.AddTransient<ITextCompletionService, TextCompletionService>();
            return services;
        }

        public static IServiceCollection AddAzureOpenAITextEmbeddings(this IServiceCollection services)
        {
            services.AddTransient<IEmbeddingsService, EmbeddingsService>();
            return services;
        }

        public static IServiceCollection AddAzureOpenAIChatCompletion(this IServiceCollection services)
        {
            services.AddTransient<IChatCompletionService, ChatCompletionService>();
            return services;
        }

        private static void ConfigureHttpClientBuilder(IHttpClientBuilder clientBuilder, Action<IHttpClientBuilder> action)
        {
            action?.Invoke(clientBuilder);
        }
    }
}
