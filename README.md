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


