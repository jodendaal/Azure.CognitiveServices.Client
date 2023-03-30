using Azure.CognitiveServices.Client.OpenAI.Models.Requests;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses;
using Azure.CognitiveServices.Client.OpenAI.Services.Interfaces;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Azure.CognitiveServices.Client.Services
{

    public class OpenAIHttpService : IOpenAIHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public OpenAIHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

       

        public async Task<OpenAIHttpResult<T, TError>> PostForm<T, TError>(string? path, Object @object)
        {
            return await ErrorHandler(async () =>
            {
                @object.Validate();
                var formData = @object.ToMultipartFormDataContent();

                var response = await _httpClient.PostAsync(path, formData);
                return await HandleResponse<T, TError>(response);
            });
        }

        public async Task<OpenAIHttpResult<T, TError>> Delete<T, TError>(string? path)
        {
            return await ErrorHandler(async () =>
            {
                var response = await _httpClient.DeleteAsync(path);
                return await HandleResponse<T, TError>(response);
            });
        }

        public async Task<OpenAIHttpResult<T, TError>> Get<T, TError>(string? path)
        {
            return await ErrorHandler(async () =>
            {
                var response = await _httpClient.GetAsync(path);
                return await HandleResponse<T, TError>(response);
            });
        }

        public async Task<OpenAIHttpResult<FileContentInfo, TError>> GetFile<TError>(string? path)
        {
            return await ErrorHandler(async () =>
            {
                var response = await _httpClient.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    var fileName = response.Content?.Headers?.ContentDisposition?.FileName?.Replace(@"""", "");
                    var fileContents = new FileContentInfo(bytes, fileName ?? "file");
                    return new OpenAIHttpResult<FileContentInfo, TError>(fileContents, response.StatusCode);
                }

                var errorResponse = await response.Content.ReadAsStringAsync();
                return new OpenAIHttpResult<FileContentInfo, TError>(new Exception(response.StatusCode.ToString(), new Exception(errorResponse)), response.StatusCode, errorResponse);
            });
        }

        public async Task<OpenAIHttpResult<T, TError>> SendRequest<T, TError>(HttpRequestMessage request)
        {
            var response = await _httpClient.SendAsync(request);
            return await HandleResponse<T, TError>(response);
        }

        public async IAsyncEnumerable<OpenAIHttpResult<T, TError>> SendRequestStream<T, TError>(HttpRequestMessage request)
        {
            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(responseStream);
                string? line = null;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (line.StartsWith("data: "))
                        line = line.Substring("data: ".Length);

                    if (!string.IsNullOrWhiteSpace(line) && line != "[DONE]")
                    {
                        var t = JsonSerializer.Deserialize<T>(line.Trim(), _jsonSerializerOptions);
                        yield return new OpenAIHttpResult<T, TError>(t, response.StatusCode);
                    }
                }
            }
        }

        public async Task<OpenAIHttpResult<T, TError>> Post<T, TError>(string? path, Object @object)
        {
            return await ErrorHandler(async () =>
            {
                @object.Validate();
                var response = await _httpClient.PostAsJsonAsync(path, @object, _jsonSerializerOptions);
                return await HandleResponse<T, TError>(response);
            });
        }

        public async IAsyncEnumerable<OpenAIHttpResult<T, TError>> PostStream<T, TError>(string? path, Object @object)
        {
            @object.Validate();

            using (HttpRequestMessage req = new(HttpMethod.Post, path))
            {
                req.Content = new StringContent(JsonSerializer.Serialize(@object, _jsonSerializerOptions), UnicodeEncoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    using var reader = new StreamReader(responseStream);
                    string? line = null;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (line.StartsWith("data: "))
                            line = line.Substring("data: ".Length);

                        if (!string.IsNullOrWhiteSpace(line) && line != "[DONE]")
                        {
                            var t = JsonSerializer.Deserialize<T>(line.Trim(), _jsonSerializerOptions);
                            yield return new OpenAIHttpResult<T, TError>(t, response.StatusCode);
                        }
                    }
                }
            }
        }

        public async Task<OpenAIHttpResult<T, TError>> HandleResponse<T, TError>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var responseObject = await response.Content.ReadFromJsonAsync<T>();
                return new OpenAIHttpResult<T, TError>(responseObject, response.StatusCode);
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            return new OpenAIHttpResult<T, TError>(new Exception(response.StatusCode.ToString(), new Exception(errorResponse)), response.StatusCode, errorResponse);
        }

        private async Task<OpenAIHttpResult<T, TError>> ErrorHandler<T, TError>(Func<Task<OpenAIHttpResult<T, TError>>> func)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                return new OpenAIHttpResult<T, TError>(ex, System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
