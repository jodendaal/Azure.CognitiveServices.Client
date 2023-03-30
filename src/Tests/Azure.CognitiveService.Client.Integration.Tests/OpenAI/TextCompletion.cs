using NUnit.Framework;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Azure.CognitiveServices.Client.OpenAI.ExtensionMethods;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Net;

namespace Azure.CognitiveService.Client.Integration.Tests.OpenAI
{
    public class TextCompletion : BaseTest
    {
        [Test]
        public async Task Completion()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfig>>().Get("textCompletion");

            var completionService = ServiceProvider.GetService<ITextCompletionService>()!;
            var completionResponse = await completionService.CreateAsync("Say This is a test.", config, options =>
            {
                options.MaxTokens = 200;
                options.N = 1;
                options.Temperature = 1;
               
            });


            Assert.That(completionResponse.IsSuccess, Is.True);
            Assert.That(completionResponse.Value!.ToString(), Is.Not.Empty);
        }

        [Test]
        public async Task CompletionImplicitConvert()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfig>>().Get("textCompletion");

            var completionService = ServiceProvider.GetService<ITextCompletionService>()!;
            TextCompletionResponse completionResponse = await completionService.CreateAsync("Say This is a test.", config, options =>
            {
                options.MaxTokens = 200;
                options.N = 1;
                options.Temperature = 1;
            });

            Console.WriteLine(completionResponse.ToString());

            Assert.That(completionResponse.ToString(), Is.Not.Empty);
        }

        [Test]
        public async Task CompletionStream()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfig>>().Get("textCompletion");

            var completionService = ServiceProvider.GetService<ITextCompletionService>()!;

            List<string> response = new List<string>();

            await foreach (var result in completionService.CreateStream("Say This is a test.", config, options =>
            {
                options.MaxTokens = 200;
                options.N = 1;
                options.Temperature = 1;

            }))
            {
                if (result.IsSuccess)
                {
                    response.Add(result.Value!.ToString());
                }

                Console.WriteLine(result?.Value?.ToString());
            }

            var responseText = string.Join(" ", response).Trim();
            Console.WriteLine($"Complete Response \r\n{responseText}");

            Assert.That(responseText, Is.Not.Empty);
            
        }


        [Test]
        public async Task CompletionUnauthorisedResponse()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfig>>().Get("textCompletion");
            var badConfig = config with { ApiKey = "" };
            var completionService = ServiceProvider.GetService<ITextCompletionService>()!;
            var completionResponse = await completionService.CreateAsync("Say This is a test.", badConfig, options =>
            {
                options.MaxTokens = 200;
                options.N = 1;
                options.Temperature = 1;
            });

            Assert.That(completionResponse.IsSuccess, Is.False);
            Assert.That(completionResponse.ErrorResponse, Is.Not.Null);
            Assert.That(completionResponse.Exception, Is.Not.Null);
            Assert.That(completionResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(completionResponse.ErrorResponse.Error.Code, Is.EqualTo("401")); 
            Assert.That(completionResponse.ErrorResponse.Error.Message.Contains("Access denied due"), Is.EqualTo(true));
        }

        [Test]
        public async Task CompletionBadRequestResponse()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfig>>().Get("textCompletion");
            var badConfig = config with { ApiUrl = "" };

            var completionService = ServiceProvider.GetService<ITextCompletionService>()!;
           
            var completionResponse = await completionService.CreateAsync("Say This is a test.", badConfig, options =>
            {
                options.MaxTokens = 200;
                options.N = 1;
                options.Temperature = 1;
            });

            Assert.That(completionResponse.IsSuccess, Is.False);
            Assert.That(completionResponse.Exception, Is.Not.Null);
            Assert.That(completionResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}