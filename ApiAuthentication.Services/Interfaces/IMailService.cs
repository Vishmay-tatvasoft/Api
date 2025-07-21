namespace ApiAuthentication.Services.Interfaces;

public interface IMailService
{
    Task<bool> SendEmail(string toemail, string emailBody);
    Task<string> GetEmailBodyAsync(string templateName);
    Task SendResetPasswordLink(string email, string username, string token);

}
