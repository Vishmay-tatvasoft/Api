using System.Net;
using System.Net.Mail;
using ApiAuthentication.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ApiAuthentication.Services.Implementations;

public class MailService(IConfiguration config) : IMailService
{
    private readonly IConfiguration _config = config;

    public async Task<bool> SendEmail(string toemail, string emailBody)
    {
        string smtpEmail = _config.GetValue<string>("SMTPCredentials:Email")!;
        string smtpappPass = _config.GetValue<string>("SMTPCredentials:AppPass")!;
        string smtphost = _config.GetValue<string>("SMTPCredentials:Host")!;

        SmtpClient smtpclient = new(smtphost)
        {
            Port = 587,
            Credentials = new NetworkCredential(smtpEmail, smtpappPass),
            EnableSsl = true,

        };

        MailMessage mail = new MailMessage
        {
            From = new MailAddress(smtpEmail),
            Subject = "Fh_Enterprice",
            Body = emailBody,
            IsBodyHtml = true,

        };
        mail.To.Add(toemail);
        smtpclient.Send(mail);
        return true;
    }

    public async Task<string> GetEmailBodyAsync(string templateName)
    {
        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "MailTemplate", templateName);
        return await File.ReadAllTextAsync(templatePath);
    }

    #region OTP
    public async Task SendOtpEmail(string email, string username, string otp)
    {
        string emailBody = await GetEmailBodyAsync("OneTimePassword.html");
        emailBody = emailBody.Replace("{{otp}}", otp);
        emailBody = emailBody.Replace("{{username}}", username);
        await SendEmail(email, emailBody);
    }
    #endregion

    #region Reset Password
    public async Task SendResetPasswordLink(string email, string username, string token)
    {
        string url = _config.GetValue<string>("RequestURL:Angular")! + $"/ResetPassword?token={Uri.EscapeDataString(token)}";
        string emailBody = await GetEmailBodyAsync("ResetPassword.html");
        emailBody = emailBody.Replace("{{resetLink}}", url);
        emailBody = emailBody.Replace("{{username}}", username);
        await SendEmail(email, emailBody);
    }
 
    public async Task SendResetPasswordMessage(string? emailAddress, string username)
    {
        if (emailAddress != null)
        {
            string emailBody = await GetEmailBodyAsync("PasswordChanged.html");
            emailBody = emailBody.Replace("{{username}}", username);
            await SendEmail(emailAddress, emailBody);
        }
    }
 
    #endregion
}
