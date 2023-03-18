using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;

namespace Azure.CognitiveServices.Client.Services.Interfaces
{
    public interface IHttpService
    {
        Task<OpenAIHttpResult<T, TError>> SendRequest<T, TError>(HttpRequestMessage request);
        IAsyncEnumerable<OpenAIHttpResult<T, TError>> SendRequestStream<T, TError>(HttpRequestMessage request);
        Task<OpenAIHttpResult<T, TError>> Delete<T, TError>(string? path);
        Task<OpenAIHttpResult<T, TError>> Get<T, TError>(string? path);
        Task<OpenAIHttpResult<FileContentInfo, TError>> GetFile<TError>(string? path);
        Task<OpenAIHttpResult<T, TError>> Post<T, TError>(string? path, object @object);
        Task<OpenAIHttpResult<T, TError>> PostForm<T, TError>(string? path, object @object);
        IAsyncEnumerable<OpenAIHttpResult<T, TError>> PostStream<T, TError>(string? path, object @object);
    }

   
}
