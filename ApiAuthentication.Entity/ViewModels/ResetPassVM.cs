using System.ComponentModel.DataAnnotations;

namespace ApiAuthentication.Entity.ViewModels;

public class ResetPassVM
{
    [Required(ErrorMessage ="Username Required")]
    public string UserName { get; set; } = string.Empty;
    [Required(ErrorMessage ="Password Required")]
    public string Password { get; set; } = string.Empty;
    [Required(ErrorMessage ="Invaid Link")]
    public string Token { get; set; } = string.Empty;
}
