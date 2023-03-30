using NUnit.Framework;
using Azure.CognitiveServices.Client.OpenAI.ExtensionMethods;
using Microsoft.Extensions.DependencyInjection;
using Azure.CognitiveServices.Client.OpenAI.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Azure.CognitiveServices.Client.OpenAI.Models;

namespace Azure.CognitiveService.Client.Integration.Tests.OpenAI
{
    public class TextEmbeddings : BaseTest
    {
        [Test]
        public async Task Embedding()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsSnapshot<AzureOpenAIConfig>>().Get("textEmbeddings");

            var embeddingService = ServiceProvider.GetRequiredService<IEmbeddingsService>();
            var embeddingsResponse = await embeddingService.Create("Some text you want to get embeddings for.",config);


            Assert.That(embeddingsResponse.IsSuccess, Is.True);
            Assert.That(embeddingsResponse.Value!.Data , Is.Not.Null);
            Assert.That(embeddingsResponse.Value.Data.Length, Is.GreaterThan(0));
        }

        [Test]
        public async Task EmbeddingImplicitConvert()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsSnapshot<AzureOpenAIConfig>>().Get("textEmbeddings");

            var embeddingService = ServiceProvider.GetRequiredService<IEmbeddingsService>();
            EmbeddingsResponse embeddingsResponse = await embeddingService.Create("Some text you want to get embeddings for.", config);

            Assert.That(embeddingsResponse, Is.Not.Null);
            Assert.That(embeddingsResponse.Data, Is.Not.Null);
            Assert.That(embeddingsResponse.Data.Length, Is.GreaterThan(0));
        }

        [Test]
        public async Task EmbeddingUnauthorisedResponse()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsSnapshot<AzureOpenAIConfig>>().Get("textEmbeddings");
            var badConfig = config with { ApiKey = "" };
            var embeddingService = ServiceProvider.GetRequiredService<IEmbeddingsService>();
            var embeddingsResponse = await embeddingService.Create("Some text you want to get embeddings for.", badConfig);


            Assert.That(embeddingsResponse.IsSuccess, Is.False);
            Assert.That(embeddingsResponse.ErrorResponse, Is.Not.Null);
            Assert.That(embeddingsResponse.Exception, Is.Not.Null);
            Assert.That(embeddingsResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(embeddingsResponse.ErrorResponse.Error.Code, Is.EqualTo("401"));
            Assert.That(embeddingsResponse.ErrorResponse.Error.Message.Contains("Access denied due"), Is.EqualTo(true));
        }


        [Test]
        public async Task EmbeddingBadRequestResponse()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsSnapshot<AzureOpenAIConfig>>().Get("textEmbeddings");
            var badConfig = config with { ApiUrl = "" };

            var embeddingService = ServiceProvider.GetRequiredService<IEmbeddingsService>();
            var embeddingsResponse = await embeddingService.Create("Some text you want to get embeddings for.", badConfig);


            Assert.That(embeddingsResponse.IsSuccess, Is.False);
            Assert.That(embeddingsResponse.Exception, Is.Not.Null);
            Assert.That(embeddingsResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}