namespace ClaimsParser.Domain.Models
{
    internal record Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Value { get; private set; }
        public string? Error { get; private set; } = string.Empty;

        private Result() { }

        public static Result<T> Ok(T value)
        {
            return new Result<T>() { IsSuccess = true, Value = value };
        }

        public static Result<T> Fail(string error)
        {
            return new Result<T>() { IsSuccess = false, Error = error };
        }
    }
}