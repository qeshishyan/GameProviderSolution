namespace UserService.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; private set; }
        public override string Message { get; }

        public ApiException(int status, string message)
        {
            StatusCode = status;
            Message = message;
        }
    }
}
