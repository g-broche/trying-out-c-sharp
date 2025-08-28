namespace TestApi.Responses;

public record ApiResponse<T>(bool IsSuccess, string? Message, T? Data) where T : class
{
    public static ApiResponse<T> Success(T data, string? message = null) =>
        new(true, message, data);

    public static ApiResponse<T> Fail(string message, T? data = default) =>
        new(false, message, data);
}
