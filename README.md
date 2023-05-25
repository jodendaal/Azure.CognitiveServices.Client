# Azure.CognitiveServices.Client
Client library for calling Azure Cognitive Services

# Getting started

Install package [Nuget package](https://www.nuget.org/packages/CognitiveServices.AI.Client)

```powershell
Install-Package CognitiveServices.AI.Client
```

Register services using the provided extension methods

```csharp
//Add Azure OpenAI
  builder.Services
     .AddAzureOpenAIHttpService(httpClientOptions => GetRetryPolicy())
     .AddAzureOpenAITextCompletion()
     .AddAzureOpenAITextEmbeddings()
     .AddAzureOpenAIChatCompletion();
```
## Streaming Method
```csharp
  var config = ServiceProvider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfig>>().Get("chat");

  var chatService = ServiceProvider.GetRequiredService<IChatCompletionService>();

  await foreach (var response in chatService.CreateStream("Say This is a test.", config, options =>
  {
      options.MaxTokens = 200;
      options.N = 1;
      options.Temperature = 1;

  }))
  {
      Console.WriteLine(response?.Value?.ToString());
  }
```

## Synchronous Method
```csharp
var config = ServiceProvider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfig>>().Get("chat");

var completionService = ServiceProvider.GetRequiredService<ITextCompletionService>();
var completionResponse = await completionService.CreateAsync("Say This is a test.", config, options =>
{
    options.MaxTokens = 200;
    options.N = 1;
    options.Temperature = 1;

});

Console.WriteLine(completionResponse.Value!.ToString());
```

## Examples

### Blazor
https://github.com/jodendaal/Azure.CognitiveServices.Client/tree/main/src/Azure.CognitiveService.Client.BlazorApp

### API
https://github.com/jodendaal/Azure.CognitiveServices.Client/tree/main/src/examples/Azure.CognitiveService.API

### Console
https://github.com/jodendaal/Azure.CognitiveServices.Client/tree/main/src/examples/Azure.CognitiveService.Client.ConsoleApp

### Functions App
https://github.com/jodendaal/Azure.CognitiveServices.Client/tree/main/src/Azure.CognitiveService.Client.FunctionsApp

### Integration Tests
https://github.com/jodendaal/Azure.CognitiveServices.Client/tree/main/src/Tests/Azure.CognitiveService.Client.Integration.Tests

## Model References
https://github.com/jodendaal/Azure.CognitiveServices.Client/tree/main/src/Azure.CognitiveServices.Client/Azure.CognitiveServices.Client/OpenAI/Models

