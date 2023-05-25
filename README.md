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

