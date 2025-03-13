namespace CustomersAPI.Api.Models
{
    public class ApiResponse<TResult>
    {
        public string Message { get; set; } = string.Empty;
        public TResult Result { get; set; }
        public object Error { get; set; }
        public bool IsSuccess { get; set; } = false;
    }
}
