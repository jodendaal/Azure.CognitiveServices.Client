using NUnit.Framework;
using Azure.CognitiveServices.Client.OpenAI.ExtensionMethods;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models;

namespace Azure.CognitiveService.Client.Integration.Tests.OpenAI
{
    public class ChatCompletion : BaseTest
    {
        [Test]
        public async Task ChatCompletionWithTextCompletionServices()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfig>>().Get("chat");

            var completionService = ServiceProvider.GetRequiredService<ITextCompletionService>();
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
        public async Task ChatCreateAsync()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfig>>().Get("chat");

            var chatService = ServiceProvider.GetRequiredService<IChatCompletionService>();

            var messages = new List<Message>
            {
                Message.Create(ChatRoleType.System, "You are a helpful assistant."),
                Message.Create(ChatRoleType.User, "Who won the world series in 2020?"),
                Message.Create(ChatRoleType.Assistant, "The Los Angeles Dodgers won the World Series in 2020."),
                Message.Create(ChatRoleType.User, "Where was it played?")
            };

            var request = new ChatCompletionRequest(messages);
            var chatResponse = await chatService.CreateAsync(request, config);


            Assert.That(chatResponse.IsSuccess, Is.True);
            Assert.That(chatResponse.Value!.Choices.Length, Is.GreaterThanOrEqualTo(1));
         
        }

        [Test]
        public async Task ChatCreateStream()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfig>>().Get("chat");

            var chatService = ServiceProvider.GetRequiredService<IChatCompletionService>();

            List<string?> responseList = new();

            await foreach (var response in chatService.CreateStream("Say This is a test.", config, options =>
            {
                options.MaxTokens = 200;
                options.N = 1;
                options.Temperature = 1;

            }))
            {
                if (response.IsSuccess)
                {
                    responseList.Add(response.Value!.ToString());
                }

                Console.WriteLine(response?.Value?.ToString());
            }

            var responseText = string.Join(" ", responseList).Trim();

            Assert.That(responseText, Is.Not.Empty);

        }



    }
}