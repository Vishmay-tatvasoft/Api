using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApiAuthentication.Entity.Models;

[Table("fh_user")]
[Index("UserId", Name = "user_id", IsUnique = true)]
public partial class FhUser
{
    [Key]
    [Column("user_id")]
    [StringLength(15)]
    public string UserId { get; set; } = null!;

    [Column("user_type")]
    [StringLength(1)]
    public string UserType { get; set; } = null!;

    [Column("role_id")]
    [StringLength(15)]
    public string? RoleId { get; set; }

    [Column("last_name")]
    [StringLength(25)]
    public string LastName { get; set; } = null!;

    [Column("first_name")]
    [StringLength(25)]
    public string FirstName { get; set; } = null!;

    [Column("user_name")]
    [StringLength(15)]
    public string UserName { get; set; } = null!;

    [Column("phone_number")]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [Column("email_address")]
    [StringLength(80)]
    public string? EmailAddress { get; set; }

    [Column("next_log_num")]
    [Precision(9, 0)]
    public decimal NextLogNum { get; set; }

    [Column("replication_flag")]
    [StringLength(15)]
    public string? ReplicationFlag { get; set; }

    [Column("logon_status")]
    [StringLength(1)]
    public string? LogonStatus { get; set; }

    [Column("logon_last_date", TypeName = "timestamp without time zone")]
    public DateTime? LogonLastDate { get; set; }

    [Column("active_user_yn")]
    [StringLength(1)]
    public string ActiveUserYn { get; set; } = null!;

    [Column("pass_expire_days")]
    public short? PassExpireDays { get; set; }

    [Column("password")]
    [StringLength(100)]
    public string Password { get; set; } = null!;

    [Column("last_logoff_date", TypeName = "timestamp without time zone")]
    public DateTime? LastLogoffDate { get; set; }
}
