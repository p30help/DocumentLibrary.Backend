namespace DocumentsLibrary.Application.Common
{
    public class OperationResult<TResult>
    {
        public TResult? Data { get; private set; }
        public bool IsNotFound { get; private set; }
        public bool IsSuccess { get; private set; }
        public string? ErrorMessage { get; private set; }

        private OperationResult() { }

        public static OperationResult<TResult> Success(TResult result)
            => new OperationResult<TResult> { Data = result, IsSuccess = true };


        public static OperationResult<TResult> Failure(string message)
            => new OperationResult<TResult> { ErrorMessage = message, IsSuccess = false };


        public static OperationResult<TResult> NotFound(string message)
            => new OperationResult<TResult> { ErrorMessage = message, IsSuccess = false, IsNotFound = true, };

    }
}