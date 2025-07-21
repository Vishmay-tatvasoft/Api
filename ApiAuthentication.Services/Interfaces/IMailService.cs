namespace ApiAuthentication.Services.Interfaces;

public interface IMailService
{
    Task<bool> SendEmail(string toemail, string emailBody);
    Task<string> GetEmailBodyAsync(string templateName);
    Task SendResetPasswordLink(string email, string username, string token);
    Task SendResetPasswordMessage(string? emailAddress, string userName);
    Task SendOtpEmail(string email, string username, string otp);
}
