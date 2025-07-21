using System.Text.Json;
using ApiAuthentication.Entity.Shared;
using ApiAuthentication.Entity.ViewModels;

namespace ApiAuthentication.Web.Middlewares;

public class GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    private readonly string _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "Exception");

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            ExceptionDTO exception = new()
            {
                Path = context.Request.Path,
                Method = context.Request.Method,
                Message = ex.Message,
                StackTrace = ex.StackTrace
            };

            WriteExceptionToFile(exception);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            ArgumentNullException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        ApiResponseVM<object> response = new ApiResponseVM<object>(statusCode, Constants.EXCEPTION_MESSAGE, null);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsync(response.ToString()!);
    }
    private void WriteExceptionToFile(ExceptionDTO dto)
    {
        try
        {
            var logDirectory = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var dailyLogFilePath = Path.Combine(logDirectory, $"{date}_exception.txt");

            var log = $"[{dto.Time}] {dto.Method} {dto.Path}\nMessage: {dto.Message}\nStackTrace: {dto.StackTrace}\n\n";
            File.AppendAllText(dailyLogFilePath, log);
        }
        catch
        {
            // Silent catch — avoid logging failure causing another crash
        }
    }
}
