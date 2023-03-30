using System.Net;

namespace Azure.CognitiveServices.Client.Models.OperationResult
{
    public class HttpResult<T> : Result<T>
    {
        public HttpResult(T? result, HttpStatusCode httpStatusCode) : base(result)
        {
            StatusCode = httpStatusCode;
        }

        public HttpResult(Exception exception, HttpStatusCode httpStatusCode, string? errorMessaage = null) : base(exception, errorMessaage)
        {
            StatusCode = httpStatusCode;
        }

        public HttpStatusCode StatusCode { get; set; }

        public static implicit operator HttpResult<T>(T? result) => new(result, HttpStatusCode.OK);
        public static implicit operator T(HttpResult<T> result) => result.Value!;
    }
}
