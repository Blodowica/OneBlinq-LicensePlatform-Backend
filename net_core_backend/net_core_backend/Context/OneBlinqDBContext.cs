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
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ActivationLogs> ActivationLogs { get; set; }
        public virtual DbSet<AccessTokens> AccessTokens { get; set; }
        public virtual DbSet<FreeTrials> FreeTrials { get; set; }
        public virtual DbSet<ActivateablePlugins> ActivateablePlugins { get; set; }
        public virtual DbSet<UniqueUsers> UniqueUsers { get; set; }


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
                    .HasColumnName("first_name")
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Role)
                    .HasColumnName("role").IsRequired().HasDefaultValue("User");

                entity.Property(e => e.Birthdate)
                .HasColumnName("date_of_birth");


                entity.Property(e => e.Address)
                .HasColumnName("address")
                .HasMaxLength(30);

                entity.Property(e => e.City)
                .HasColumnName("city")
                .HasMaxLength(20);

                entity.Property(e => e.PostalCode)
                .HasColumnName("postal_code")
                .HasMaxLength(10);

                entity.Property(e => e.Country)
                .HasColumnName("country")
                .HasMaxLength(30);

                entity.Property(e => e.GumroadID)
                    .HasColumnName("gumroad_id");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(250);

                entity.Property(e => e.AbuseNotifications)
                .HasColumnName("send_abuse_notifications");

            });

            modelBuilder.Entity<RefreshTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.Id });

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Licenses>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnName("created_at");

                entity.Property(e => e.ExpiresAt)
                    .HasColumnName("expires_at");

                entity.Property(e => e.PurchaseLocation)
                    .HasColumnName("purchase_location")
                    .HasMaxLength(100);

                entity.Property(e => e.LicenseKey)
                    .HasColumnName("license_key")
                    .IsRequired();

                entity.Property(e => e.GumroadSaleID)
                    .HasColumnName("gumroad_sale_id");

                entity.Property(e => e.GumroadSubscriptionID)
                    .HasColumnName("gumroad_subscription_id");

                entity.Property(e => e.Recurrence)
                    .HasColumnName("recurrence")
                    .HasMaxLength(20);

                entity.Property(e => e.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(10);

                entity.Property(e => e.Price)
                    .HasColumnName("price");

                entity.Property(e => e.EndedReason)
                    .HasColumnName("ended_reason")
                    .HasMaxLength(100);

                entity.Property(e => e.RestartedAt)
                    .HasColumnName("restarted_at");

                entity.HasOne(l => l.User)
                    .WithMany(u => u.Licenses)
                    .HasForeignKey(l => l.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Licenses_Users");

                entity.HasOne(l => l.Product)
                    .WithMany(p => p.Licenses)
                    .HasForeignKey(l => l.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Licenses_Products");                    
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnName("product_name")
                    .HasMaxLength(100);

                entity.Property(e => e.VariantName)
                    .HasColumnName("variant_name");

                entity.Property(e => e.MaxUses)
                    .HasColumnName("max_uses");

                entity.Property(e => e.Active)
                    .HasColumnName("active");

                entity.Property(e => e.GumroadID)
                    .HasColumnName("gumroad_id");
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
                    .HasColumnName("successful");

                entity.Property(e => e.UniqueUserId)
                    .IsRequired()
                    .HasColumnName("unique_user_id");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message");

                entity.HasOne(al => al.License)
                    .WithMany(l => l.ActivationLogs)
                    .HasForeignKey(al => al.LicenseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActivationLogs_Licenses");

                entity.HasOne(e => e.UniqueUser)
                    .WithMany(e => e.ActivationLogs)
                    .HasForeignKey(entity => entity.UniqueUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActivationLogs_UniqueUser");
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

            modelBuilder.Entity<FreeTrials>(entity =>
            {
                entity.Property(e => e.Id)
                .HasColumnName("id");

                entity.Property(e => e.PluginName)
                .IsRequired()
                .HasColumnName("plugin_name");

                entity.Property(e => e.StartDate)
                .IsRequired()
                .HasColumnName("start_date");

                entity.Property(e => e.EndDate)
                .IsRequired()
                .HasColumnName("end_date");

                entity.Property(e => e.UniqueUserId)
                    .IsRequired()
                    .HasColumnName("unique_user_id");

                entity.HasOne(e => e.UniqueUser)
                    .WithMany(e => e.FreeTrials)
                    .HasForeignKey(entity => entity.UniqueUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FreeTrials_UniqueUser");
            });

            modelBuilder.Entity<ActivateablePlugins>(entity =>
            {
                entity.Property(ap => ap.Id)
                    .HasColumnName("id");

                entity.Property(ap => ap.Plugin)
                    .IsRequired()
                    .HasColumnName("plugin");

                entity.HasOne(ap => ap.Product)
                    .WithMany(p => p.ActivateablePlugins)
                    .HasForeignKey(p => p.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActivateablePlugins_Products");
            });

            modelBuilder.Entity<UniqueUsers>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.ExternalServiceName)
                    .IsRequired()
                    .HasColumnName("service");

                entity.Property(e => e.ExternalUserServiceId)
                    .IsRequired()
                    .HasColumnName("external_user_Id");

            });

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
