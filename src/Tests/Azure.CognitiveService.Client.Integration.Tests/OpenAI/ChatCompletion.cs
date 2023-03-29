using NUnit.Framework;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Azure.CognitiveServices.Client.OpenAI.ExtensionMethods;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Azure.CognitiveServices.Client.OpenAI.Models.Requests;

namespace Azure.CognitiveService.Client.Integration.Tests.OpenAI
{
    public class ChatCompletion : BaseTest
    {
        [Test]
        public async Task ChatCompletionWithTextCompletionServices()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfig>>().Get("chat");

            var completionService = ServiceProvider.GetService<ITextCompletionService>()!;
            var completionResponse = await completionService.CreateAsync("Say This is a test.", config, options =>
            {
                options.MaxTokens = 200;
                options.N = 1;
                options.Temperature = 1;
               
            });


            Assert.That(completionResponse.IsSuccess, Is.True);
            Assert.That(completionResponse.Value!.ToString().Trim().Contains("This is a test"), Is.EqualTo(true));
        }

        [Test]
        public async Task ChatCompletionTest()
        {
            var config = ServiceProvider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfig>>().Get("chat");

            var completionService = ServiceProvider.GetService<IChatCompletionService>()!;

            var messages = new List<Message>
            {
                Message.Create(ChatRoleType.System, "You are a helpful assistant."),
                Message.Create(ChatRoleType.User, "Who won the world series in 2020?"),
                Message.Create(ChatRoleType.Assistant, "The Los Angeles Dodgers won the World Series in 2020."),
                Message.Create(ChatRoleType.User, "Where was it played?")
            };

            var request = new ChatCompletionRequest(messages);
            var completionResponse = await completionService.CreateAsync(request, config);


            Assert.That(completionResponse.IsSuccess, Is.True);
            Assert.That(completionResponse.Value.Choices.Length, Is.GreaterThanOrEqualTo(1));
         
        }

        //[Test]
        //public async Task CompletionImplicitConvert()
        //{
        //    var config = ServiceProvider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfig>>().Get("textCompletion");

        //    var completionService = ServiceProvider.GetService<ITextCompletionService>()!;
        //    TextCompletionResponse completionResponse = await completionService.Get("Say This is a test.", config, options =>
        //    {
        //        options.MaxTokens = 200;
        //        options.N = 1;
        //        options.Temperature = 1;
        //    });

        //    Console.WriteLine(completionResponse.ToString());

        //    Assert.That(completionResponse.ToString().Trim().Contains("This is a test"), Is.EqualTo(true));
        //}

        //[Test]
        //public async Task CompletionStream()
        //{
        //    var config = ServiceProvider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfig>>().Get("textCompletion");

        //    var completionService = ServiceProvider.GetService<ITextCompletionService>()!;

        //    List<string> response = new List<string>();

        //    await foreach (var result in completionService.GetStream("Say This is a test.", config, options =>
        //    {
        //        options.MaxTokens = 200;
        //        options.N = 1;
        //        options.Temperature = 1;

        //    }))
        //    {
        //        if (result.IsSuccess)
        //        {
        //            response.Add(result.Value!.ToString());
        //        }

        //        Console.WriteLine(result?.Value?.ToString());
        //    }

        //    var responseText = string.Join(" ", response).Trim();
        //    Console.WriteLine($"Complete Response \r\n{responseText}");

        //    Assert.That(responseText, Is.EqualTo("This  is  a  test ."));

        //}


        //[Test]
        //public async Task CompletionUnauthorisedResponse()
        //{
        //    var config = ServiceProvider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfig>>().Get("textCompletion");
        //    var badConfig = config with { ApiKey = "" };
        //    var completionService = ServiceProvider.GetService<ITextCompletionService>()!;
        //    var completionResponse = await completionService.Get("Say This is a test.", badConfig, options =>
        //    {
        //        options.MaxTokens = 200;
        //        options.N = 1;
        //        options.Temperature = 1;
        //    });

        //    Assert.That(completionResponse.IsSuccess, Is.False);
        //    Assert.That(completionResponse.ErrorResponse, Is.Not.Null);
        //    Assert.That(completionResponse.Exception, Is.Not.Null);
        //    Assert.That(completionResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        //    Assert.That(completionResponse.ErrorResponse.Error.Code, Is.EqualTo("401")); 
        //    Assert.That(completionResponse.ErrorResponse.Error.Message.Contains("Access denied due"), Is.EqualTo(true));
        //}

        //[Test]
        //public async Task CompletionBadRequestResponse()
        //{
        //    var config = ServiceProvider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfig>>().Get("textCompletion");
        //    var badConfig = config with { ApiUrl = "" };

        //    var completionService = ServiceProvider.GetService<ITextCompletionService>()!;

        //    var completionResponse = await completionService.Get("Say This is a test.", badConfig, options =>
        //    {
        //        options.MaxTokens = 200;
        //        options.N = 1;
        //        options.Temperature = 1;
        //    });

        //    Assert.That(completionResponse.IsSuccess, Is.False);
        //    Assert.That(completionResponse.Exception, Is.Not.Null);
        //    Assert.That(completionResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        //}
    }
}