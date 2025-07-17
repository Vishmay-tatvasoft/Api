namespace ApiAuthentication.Entity.ViewModels;

public class ApiResponseVM<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public ApiResponseVM(int statusCode, string message, T data)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
    }
}
