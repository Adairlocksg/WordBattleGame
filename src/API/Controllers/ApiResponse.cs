namespace WordBattle.API.Controllers
{
    public class ApiResponse<T>(bool success, string message, T? content, string? errorCode)
    {
        public bool Success { get; set; } = success;
        public string Message { get; set; } = message;
        public T? Content { get; set; } = content;
        public string? ErrorCode { get; set; } = errorCode;
    }
}
