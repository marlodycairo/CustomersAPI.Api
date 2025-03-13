namespace CustomersAPI.Api.Models
{
    public class GenericResponse<TResult>
    {
        public GenericResponse()
        {
        }

        public GenericResponse(string message, TResult result, object error, bool isSuccess)
        {
            Message = message;
            Result = result;
            Error = error;
            IsSuccess = isSuccess;
        }

        public string Message { get; set; } = string.Empty;
        public TResult Result { get; set; }
        public object Error { get; set; }
        public bool IsSuccess { get; set; } = false;
    }
}
