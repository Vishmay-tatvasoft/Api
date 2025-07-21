using System.ComponentModel.DataAnnotations;

namespace ApiAuthentication.Entity.ViewModels;

public class SignupVM
{
    [StringLength(30)]
    public string EmailAddress { get; set; }
    [StringLength(15)]
    public string RoleId { get; set; }
    [StringLength(25)]
    public string LastName { get; set; } = null!;
    [StringLength(25)]
    public string FirstName { get; set; } = null!;
    [StringLength(15)]
    public string UserName { get; set; } = null!;
}
