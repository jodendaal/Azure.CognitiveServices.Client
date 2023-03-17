using NUnit.Framework;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Azure.CognitiveServices.Client.OpenAI.ExtensionMethods;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Azure.CognitiveService.Client.Integration.Tests.OpenAI
{
    public class TextCompletion : BaseTest
    {
        [Test]
        public async Task Completion()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsSnapshot<AzureOpenAIConfig>>().Get("textCompletion");

            var completionService = ServiceProvider.GetService<ITextCompletionService>()!;
            var completionResponse = await completionService.Get("Say This is a test.", config, options =>
            {
                options.MaxTokens = 200;
                options.N = 1;
                options.Temperature = 1;
               
            });


            Assert.That(completionResponse.IsSuccess, Is.True);
            Assert.That(completionResponse.Result!.Choices[0].Text.Trim(), Is.EqualTo("This is a test."));
        }

        [Test]
        public async Task CompletionStream()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsSnapshot<AzureOpenAIConfig>>().Get("textCompletion");

            var completionService = ServiceProvider.GetService<ITextCompletionService>()!;

            List<string> response = new List<string>();

            await foreach (var result in completionService.GetStream("Say This is a test.", config, options =>
            {
                options.MaxTokens = 200;
                options.N = 1;
                options.Temperature = 1;

            }))
            {
                if (result.IsSuccess)
                {
                    response.Add(result.Result!.Choices[0].Text);
                }

                Console.WriteLine(result?.Result?.Choices[0].Text);
            }

            var responseText = string.Join(" ", response).Trim();
            Console.WriteLine($"Complete Response \r\n{responseText}");

            Assert.That(responseText, Is.EqualTo("This  is  a  test ."));
            
        }
    }
}