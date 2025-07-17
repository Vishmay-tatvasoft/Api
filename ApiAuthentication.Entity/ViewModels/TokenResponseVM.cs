namespace ApiAuthentication.Entity.ViewModels;

public class TokenResponseVM
{
    public string Email { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public bool RememberMe { get; set; }
}
