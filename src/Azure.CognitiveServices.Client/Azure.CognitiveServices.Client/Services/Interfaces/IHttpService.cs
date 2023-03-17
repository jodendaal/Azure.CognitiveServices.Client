using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;

namespace Azure.CognitiveServices.Client.Services.Interfaces
{
    public interface IHttpService
    {
        Task<OpenAIHttpOperationResult<T, TError>> SendRequest<T, TError>(HttpRequestMessage request);
        IAsyncEnumerable<OpenAIHttpOperationResult<T, TError>> SendRequestStream<T, TError>(HttpRequestMessage request);
        Task<OpenAIHttpOperationResult<T, TError>> Delete<T, TError>(string? path);
        Task<OpenAIHttpOperationResult<T, TError>> Get<T, TError>(string? path);
        Task<OpenAIHttpOperationResult<FileContentInfo, TError>> GetFile<TError>(string? path);
        Task<OpenAIHttpOperationResult<T, TError>> Post<T, TError>(string? path, object @object);
        Task<OpenAIHttpOperationResult<T, TError>> PostForm<T, TError>(string? path, object @object);
        IAsyncEnumerable<OpenAIHttpOperationResult<T, TError>> PostStream<T, TError>(string? path, object @object);
    }

   
}
