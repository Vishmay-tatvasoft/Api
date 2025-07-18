using System;
using System.Collections.Generic;
using ApiAuthentication.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiAuthentication.Entity.Data;

public partial class TatvasoftFhContext : DbContext
{
    public TatvasoftFhContext()
    {
    }

    public TatvasoftFhContext(DbContextOptions<TatvasoftFhContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FhSystem> FhSystems { get; set; }

    public virtual DbSet<FhUser> FhUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=172.16.10.23;Database=Tatvasoft_FH;Username=parthesh; password=parthesh;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FhUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("fh_user_pkey");

            entity.Property(e => e.ActiveUserYn).HasDefaultValueSql("'Y'::character varying");
            entity.Property(e => e.UserType).HasDefaultValueSql("'U'::character varying");
        });
        modelBuilder.HasSequence("user_suffix_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
