using NUnit.Framework;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Azure.CognitiveServices.Client.OpenAI.ExtensionMethods;
using Microsoft.Extensions.DependencyInjection;
using Azure.CognitiveServices.Client.OpenAI.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Azure.CognitiveService.Client.Integration.Tests.OpenAI
{
    public class TextEmbeddings : BaseTest
    {
        [Test]
        public async Task Embedding()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsSnapshot<AzureOpenAIConfig>>().Get("textEmbeddings");

            var embeddingService = ServiceProvider.GetService<IEmbeddingsService>()!;
            var embeddingsResponse = await embeddingService.Create("Some text you want to get embeddings for.",config);


            Assert.That(embeddingsResponse.IsSuccess, Is.True);
            Assert.That(embeddingsResponse.Result!.Data , Is.Not.Null);
            Assert.That(embeddingsResponse.Result.Data.Length, Is.GreaterThan(0));
        }
    }
}