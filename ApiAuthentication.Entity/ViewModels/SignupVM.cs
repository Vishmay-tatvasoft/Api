using System.ComponentModel.DataAnnotations;

namespace ApiAuthentication.Entity.ViewModels;

public class SignupVM
{
    [StringLength(1)]
    public string UserType { get; set; } = null!;
    [StringLength(15)]
    public string? RoleId { get; set; }
    [StringLength(25)]
    public string LastName { get; set; } = null!;
    [StringLength(25)]
    public string FirstName { get; set; } = null!;
    [StringLength(15)]
    public string UserName { get; set; } = null!;
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
    [StringLength(80)]
    public string? EmailAddress { get; set; }
    [StringLength(100)]
    public string Password { get; set; } = null!;
}
