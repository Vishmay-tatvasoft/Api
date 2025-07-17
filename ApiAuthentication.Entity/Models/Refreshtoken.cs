using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApiAuthentication.Entity.Models;

[Table("refreshtokens")]
public partial class Refreshtoken
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("token")]
    public string Token { get; set; } = null!;

    [Column("expires_at", TypeName = "timestamp without time zone")]
    public DateTime ExpiresAt { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by_ip")]
    [StringLength(50)]
    public string? CreatedByIp { get; set; }

    [Column("revoked_at", TypeName = "timestamp without time zone")]
    public DateTime? RevokedAt { get; set; }

    [Column("revoked_by_ip")]
    [StringLength(50)]
    public string? RevokedByIp { get; set; }

    [Column("replaced_by_token")]
    public string? ReplacedByToken { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Refreshtokens")]
    public virtual User User { get; set; } = null!;
}
