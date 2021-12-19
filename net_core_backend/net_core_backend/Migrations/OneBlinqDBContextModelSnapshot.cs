﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using net_core_backend.Models;

namespace net_core_backend.Migrations
{
    [DbContext(typeof(OneBlinqDBContext))]
    partial class OneBlinqDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("net_core_backend.Models.AccessTokens", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnName("access_token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Active")
                        .HasColumnName("active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AccessTokens");
                });

            modelBuilder.Entity("net_core_backend.Models.ActivateablePlugins", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Plugin")
                        .IsRequired()
                        .HasColumnName("plugin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ActivateablePlugins");
                });

            modelBuilder.Entity("net_core_backend.Models.ActivationLogs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LicenseId")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnName("message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Successful")
                        .HasColumnName("successful")
                        .HasColumnType("bit");

                    b.Property<int?>("UniqueUserId")
                        .IsRequired()
                        .HasColumnName("unique_user_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LicenseId");

                    b.HasIndex("UniqueUserId");

                    b.ToTable("ActivationLogs");
                });

            modelBuilder.Entity("net_core_backend.Models.ForgottenPasswordTokens", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("VerifiedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ForgottenPasswordTokens");
                });

            modelBuilder.Entity("net_core_backend.Models.FreeTrials", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnName("active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("EndDate")
                        .HasColumnName("end_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("FigmaUserId")
                        .IsRequired()
                        .HasColumnName("figma_user_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PluginName")
                        .IsRequired()
                        .HasColumnName("plugin_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnName("start_date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("FreeTrials");
                });

            modelBuilder.Entity("net_core_backend.Models.Licenses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .HasColumnName("currency")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("EndedReason")
                        .HasColumnName("ended_reason")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("ExpiresAt")
                        .HasColumnName("expires_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("GumroadSaleID")
                        .HasColumnName("gumroad_sale_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GumroadSubscriptionID")
                        .HasColumnName("gumroad_subscription_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LicenseKey")
                        .IsRequired()
                        .HasColumnName("license_key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnName("price")
                        .HasColumnType("real");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("PurchaseLocation")
                        .HasColumnName("purchase_location")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Recurrence")
                        .HasColumnName("recurrence")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<DateTime?>("RestartedAt")
                        .HasColumnName("restarted_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Licenses");
                });

            modelBuilder.Entity("net_core_backend.Models.Products", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnName("active")
                        .HasColumnType("bit");

                    b.Property<string>("GumroadID")
                        .HasColumnName("gumroad_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxUses")
                        .HasColumnName("max_uses")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnName("product_name")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("VariantName")
                        .HasColumnName("variant_name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("net_core_backend.Models.RefreshTokens", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedByIp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReplacedByToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RevokedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("RevokedByIp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("net_core_backend.Models.UniqueUsers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ExternalServiceName")
                        .IsRequired()
                        .HasColumnName("service")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExternalUserServiceId")
                        .IsRequired()
                        .HasColumnName("external_user_Id")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UniqueUsers");
                });

            modelBuilder.Entity("net_core_backend.Models.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AbuseNotifications")
                        .HasColumnName("send_abuse_notifications")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .HasColumnName("address")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("Birthdate")
                        .HasColumnName("date_of_birth")
                        .HasColumnType("datetime2");

                    b.Property<string>("City")
                        .HasColumnName("city")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Country")
                        .HasColumnName("country")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("FirstName")
                        .HasColumnName("first_name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("GumroadID")
                        .HasColumnName("gumroad_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnName("last_name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .HasColumnName("password")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("PostalCode")
                        .HasColumnName("postal_code")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Role")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnName("role")
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("User");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("net_core_backend.Models.AccessTokens", b =>
                {
                    b.HasOne("net_core_backend.Models.Users", "User")
                        .WithMany("AccessTokens")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_AccessTokens_Users")
                        .IsRequired();
                });

            modelBuilder.Entity("net_core_backend.Models.ActivateablePlugins", b =>
                {
                    b.HasOne("net_core_backend.Models.Products", "Product")
                        .WithMany("ActivateablePlugins")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_ActivateablePlugins_Products")
                        .IsRequired();
                });

            modelBuilder.Entity("net_core_backend.Models.ActivationLogs", b =>
                {
                    b.HasOne("net_core_backend.Models.Licenses", "License")
                        .WithMany("ActivationLogs")
                        .HasForeignKey("LicenseId")
                        .HasConstraintName("FK_ActivationLogs_Licenses");

                    b.HasOne("net_core_backend.Models.UniqueUsers", "UniqueUser")
                        .WithMany("ActivationLogs")
                        .HasForeignKey("UniqueUserId")
                        .HasConstraintName("FK_ActivationLogs_UniqueUser")
                        .IsRequired();
                });

            modelBuilder.Entity("net_core_backend.Models.ForgottenPasswordTokens", b =>
                {
                    b.HasOne("net_core_backend.Models.Users", "User")
                        .WithMany("ForgottenPasswordTokens")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_User_ForgottenPass")
                        .IsRequired();
                });

            modelBuilder.Entity("net_core_backend.Models.Licenses", b =>
                {
                    b.HasOne("net_core_backend.Models.Products", "Product")
                        .WithMany("Licenses")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_Licenses_Products")
                        .IsRequired();

                    b.HasOne("net_core_backend.Models.Users", "User")
                        .WithMany("Licenses")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_Licenses_Users")
                        .IsRequired();
                });

            modelBuilder.Entity("net_core_backend.Models.RefreshTokens", b =>
                {
                    b.HasOne("net_core_backend.Models.Users", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
