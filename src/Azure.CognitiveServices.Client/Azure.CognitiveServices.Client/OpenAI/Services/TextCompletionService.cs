﻿using Azure.CognitiveServices.Client;
using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using Azure.CognitiveServices.Client.OpenAI.Services;
using Azure.CognitiveServices.Client.OpenAI.Services.Interfaces;

public class TextCompletionService : BaseOpenAIService,ITextCompletionService
{
    private readonly IOpenAIHttpService _httpService;

    public TextCompletionService(IOpenAIHttpService httpService)
    {
        _httpService = httpService;
    }
    public Task<OpenAIHttpResult<TextCompletionResponse, ErrorResponse>> CreateAsync(TextCompletionRequest completionRequest, AzureOpenAIConfig azureOpenAIConfig)
    {
        return ErrorHandler(() =>
        {
            completionRequest.Validate();

            var request = CreateRequest(
            $"{azureOpenAIConfig.ApiUrl}/openai/deployments/{azureOpenAIConfig.DeploymentName}/completions?api-version={azureOpenAIConfig.ApiVersion}",
                azureOpenAIConfig,
                completionRequest);
            
                return _httpService.SendRequest<TextCompletionResponse, ErrorResponse>(request);
        });
    }

    public IAsyncEnumerable<OpenAIHttpResult<TextCompletionResponse, ErrorResponse>> CreateStream(TextCompletionRequest completionRequest, AzureOpenAIConfig azureOpenAIConfig)
    {
        completionRequest.Stream = true;

        completionRequest.Validate();

        var request = CreateRequest(
            $"{azureOpenAIConfig.ApiUrl}/openai/deployments/{azureOpenAIConfig.DeploymentName}/completions?api-version={azureOpenAIConfig.ApiVersion}",
            azureOpenAIConfig,
            completionRequest);


            return _httpService.SendRequestStream<TextCompletionResponse, ErrorResponse>(request);

    }
}