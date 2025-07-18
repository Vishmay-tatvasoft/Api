namespace ApiAuthentication.Entity.ViewModels;

public class ExceptionDTO
{
    public string Path { get; set; }
    public string Method { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }
    public DateTime Time { get; set; } = DateTime.Now;  
}
