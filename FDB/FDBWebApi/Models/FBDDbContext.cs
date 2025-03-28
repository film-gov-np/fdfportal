using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FDBWebApi.Models;

public partial class FBDDbContext : DbContext
{
    public FBDDbContext()
    {
    }

    public FBDDbContext(DbContextOptions<FBDDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<BrandMvc> BrandMvcs { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieMvc> MovieMvcs { get; set; }

    public virtual DbSet<ReceiptUpload> ReceiptUploads { get; set; }

    public virtual DbSet<StatusMaster> StatusMasters { get; set; }

    public virtual DbSet<Theater> Theaters { get; set; }

    public virtual DbSet<Theatre> Theatres { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.TheatreId, "IX_AspNetUsers_TheatreId");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Discriminator)
                .HasMaxLength(21)
                .HasDefaultValue("");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasOne(d => d.Theatre).WithMany(p => p.AspNetUsers).HasForeignKey(d => d.TheatreId);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandCode);

            entity.ToTable("Brand");

            entity.Property(e => e.BrandCode).HasMaxLength(25);
            entity.Property(e => e.AddedBy).HasMaxLength(256);
            entity.Property(e => e.AddedOn).HasColumnType("datetime");
            entity.Property(e => e.ApiUsername).HasMaxLength(256);
            entity.Property(e => e.BrandId)
                .ValueGeneratedOnAdd()
                .HasColumnName("BrandID");
            entity.Property(e => e.BrandName).HasMaxLength(256);
            entity.Property(e => e.DeletedBy).HasMaxLength(256);
            entity.Property(e => e.DeletedOn).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(1000);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedBy).HasMaxLength(256);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<BrandMvc>(entity =>
        {
            entity.ToTable("BrandMVCs");

            entity.Property(e => e.BrandId).HasColumnName("BrandID");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.MovieCode);

            entity.ToTable("Movie");

            entity.Property(e => e.MovieCode).HasMaxLength(25);
            entity.Property(e => e.AddedBy).HasMaxLength(50);
            entity.Property(e => e.AddedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedBy).HasMaxLength(50);
            entity.Property(e => e.DeletedOn).HasColumnType("datetime");
            entity.Property(e => e.GradeCode).HasMaxLength(25);
            entity.Property(e => e.LanguageCode).HasMaxLength(25);
            entity.Property(e => e.MovieId)
                .ValueGeneratedOnAdd()
                .HasColumnName("MovieID");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.ProductionHouseCode).HasMaxLength(25);
            entity.Property(e => e.Slug).HasMaxLength(250);
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<MovieMvc>(entity =>
        {
            entity.ToTable("MovieMVCs");

            entity.Property(e => e.MovieId).HasColumnName("MovieID");
        });

        modelBuilder.Entity<ReceiptUpload>(entity =>
        {
            entity.HasIndex(e => e.TheatreId, "IX_ReceiptUploads_TheatreId");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Theatre).WithMany(p => p.ReceiptUploads).HasForeignKey(d => d.TheatreId);
        });

        modelBuilder.Entity<Theater>(entity =>
        {
            entity.HasKey(e => e.TheaterCode);

            entity.ToTable("Theater");

            entity.Property(e => e.TheaterCode).HasMaxLength(25);
            entity.Property(e => e.AddedBy).HasMaxLength(50);
            entity.Property(e => e.AddedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Address).HasMaxLength(1000);
            entity.Property(e => e.ApiUsername).HasMaxLength(256);
            entity.Property(e => e.BrandCode).HasMaxLength(25);
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.DeletedBy).HasMaxLength(50);
            entity.Property(e => e.DeletedOn).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(1000);
            entity.Property(e => e.EncryptionKey).HasMaxLength(50);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsModified).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Pannumber)
                .HasMaxLength(50)
                .HasColumnName("PANNumber");
            entity.Property(e => e.RegNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StateId).HasColumnName("StateID");
            entity.Property(e => e.TheaterId)
                .ValueGeneratedOnAdd()
                .HasColumnName("TheaterID");
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.Vatnumber)
                .HasMaxLength(50)
                .HasColumnName("VATNumber");
        });

        modelBuilder.Entity<Theatre>(entity =>
        {
            entity.Property(e => e.Pannumber).HasColumnName("PANNumber");
            entity.Property(e => e.Vatnumber).HasColumnName("VATNumber");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
