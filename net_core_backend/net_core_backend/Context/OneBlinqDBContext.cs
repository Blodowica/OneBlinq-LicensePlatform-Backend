using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace net_core_backend.Models
{
    public partial class OneBlinqDBContext : DbContext
    {
        public OneBlinqDBContext()
        {
        }

        public OneBlinqDBContext(DbContextOptions<OneBlinqDBContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }
        public virtual DbSet<Licenses> Licenses { get; set; }
        public virtual DbSet<LicenseProducts> LicenseProducts { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ActivationLogs> ActivationLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(200);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Admin)
                    .HasColumnName("admin");

                entity.Property(e => e.GumroadID)
                    .HasColumnName("gumroad_id");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<RefreshTokens>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasColumnName("token");

                entity.Property(e => e.ExpiresAt)
                    .IsRequired()
                    .HasColumnName("expires_at");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnName("created_at");

                entity.Property(e => e.RevokedAt)
                    .IsRequired()
                    .HasColumnName("revoked_at");

                entity.Property(e => e.Active)
                    .HasColumnName("active");

                entity.HasOne(t => t.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefreshTokens_Users");
            });

            modelBuilder.Entity<Licenses>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnName("created_at");

                entity.Property(e => e.ExpiresAt)
                    .IsRequired()
                    .HasColumnName("expires_at");

                entity.Property(e => e.PurchaseLocation)
                    .HasColumnName("purchase_location")
                    .HasMaxLength(100);

                entity.Property(e => e.LicenseKey)
                    .HasColumnName("license_key")
                    .IsRequired();

                entity.Property(e => e.Active)
                    .HasColumnName("active");

                entity.Property(e => e.GumroadID)
                    .HasColumnName("gumroad_id");

                entity.HasOne(l => l.User)
                    .WithMany(u => u.Licenses)
                    .HasForeignKey(l => l.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Licenses_Users");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnName("product_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnName("price");

                entity.Property(e => e.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(10);

                entity.Property(e => e.Recurrance)
                    .HasColumnName("recurrance")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Active)
                    .HasColumnName("active");

                entity.Property(e => e.GumroadID)
                    .HasColumnName("gumroad_id");
            });

            modelBuilder.Entity<LicenseProducts>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.HasOne(lp => lp.License)
                    .WithMany(l => l.LicenseProducts)
                    .HasForeignKey(lp => lp.LicenseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LicenseProducts_Licenses");

                entity.HasOne(lp => lp.Product)
                    .WithMany(p => p.LicenseProducts)
                    .HasForeignKey(lp => lp.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LicenseProducts_Products");
            });

            modelBuilder.Entity<ActivationLogs>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnName("created_at");

                entity.Property(e => e.Successful)
                    .IsRequired()
                    .HasColumnName("successfull");

                entity.HasOne(al => al.License)
                    .WithMany(l => l.ActivationLogs)
                    .HasForeignKey(al => al.LicenseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActivationLogs_Licenses");
            });

            modelBuilder.Entity<AccessTokens>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.AccessToken)
                    .IsRequired()
                    .HasColumnName("access_token");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnName("created_at");

                entity.Property(e => e.Active)
                    .HasColumnName("active");

                entity.HasOne(t => t.User)
                    .WithMany(u => u.AccessTokens)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccessTokens_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
