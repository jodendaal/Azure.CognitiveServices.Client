namespace Azure.CognitiveServices.Client.Models.OperationResult
{
    public class Result<T>
    {
        public Result(T? result)
        {
            Value = result;
        }

        public Result(Exception exception, string? errorMessaage = null)
        {
            Exception = exception;
            ErrorMessage = errorMessaage ?? exception.Message;
        }
        public T? Value { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
        public bool IsSuccess => Exception == null;

        public static implicit operator Result<T>(T? result) => new Result<T>(result);
        public static implicit operator T(Result<T> result) => result.Value;
    }
}
