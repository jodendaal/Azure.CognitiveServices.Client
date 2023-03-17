﻿using Azure.CognitiveServices.Client.Models.OperationResult;
using Azure.CognitiveServices.Client.OpenAI.Models.Responses.Common;
using System.Net;
using System.Text.Json;

namespace Azure.CognitiveServices.Client.OpenAI.Models.Responses
{
    public class OpenAIHttpOperationResult<T, TError> : HttpOperationResult<T>
    {
        public OpenAIHttpOperationResult(T? result, HttpStatusCode httpStatusCode) : base(result, httpStatusCode)
        {
        }

        public OpenAIHttpOperationResult(Exception exception, HttpStatusCode httpStatusCode, string? errorMessaage = null) : base(exception, httpStatusCode, errorMessaage)
        {
            if (!string.IsNullOrWhiteSpace(errorMessaage))
            {
                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                ErrorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorMessaage, serializeOptions);
            }
        }

        public ErrorResponse? ErrorResponse { get; internal set; }

        public static implicit operator OpenAIHttpOperationResult<T, TError>(T? result) => new OpenAIHttpOperationResult<T, TError>(result, HttpStatusCode.OK);
        public static implicit operator T(OpenAIHttpOperationResult<T, TError> result) => result.Result;
    }
}
