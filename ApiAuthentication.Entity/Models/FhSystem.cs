using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApiAuthentication.Entity.Models;

[Keyless]
[Table("fh_system")]
public partial class FhSystem
{
    [Column("site_code")]
    [StringLength(3)]
    public string? SiteCode { get; set; }

    [Column("site_name")]
    [StringLength(50)]
    public string? SiteName { get; set; }

    [Column("site_address")]
    [StringLength(50)]
    public string? SiteAddress { get; set; }

    [Column("site_city")]
    [StringLength(25)]
    public string? SiteCity { get; set; }

    [Column("site_state")]
    [StringLength(2)]
    public string? SiteState { get; set; }
}
