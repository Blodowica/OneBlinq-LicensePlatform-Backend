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

                    b.Property<int>("LicenseId")
                        .HasColumnType("int");

                    b.Property<bool>("Successful")
                        .HasColumnName("successfull")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("LicenseId");

                    b.ToTable("ActivationLogs");
                });

            modelBuilder.Entity("net_core_backend.Models.LicenseProducts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LicenseId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LicenseId");

                    b.HasIndex("ProductId");

                    b.ToTable("LicenseProducts");
                });

            modelBuilder.Entity("net_core_backend.Models.Licenses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnName("active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnName("expires_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("GumroadID")
                        .HasColumnName("gumroad_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LicenseKey")
                        .IsRequired()
                        .HasColumnName("license_key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PurchaseLocation")
                        .HasColumnName("purchase_location")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

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

                    b.Property<string>("Currency")
                        .HasColumnName("currency")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("GumroadID")
                        .HasColumnName("gumroad_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnName("price")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnName("product_name")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Recurrance")
                        .IsRequired()
                        .HasColumnName("recurrance")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("net_core_backend.Models.RefreshTokens", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnName("active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnName("expires_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("RevokedAt")
                        .HasColumnName("revoked_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnName("token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("net_core_backend.Models.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Admin")
                        .HasColumnName("admin")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnName("first_name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("GumroadID")
                        .HasColumnName("gumroad_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnName("last_name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .HasColumnName("password")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("net_core_backend.Models.ActivationLogs", b =>
                {
                    b.HasOne("net_core_backend.Models.Licenses", "License")
                        .WithMany("ActivationLogs")
                        .HasForeignKey("LicenseId")
                        .HasConstraintName("FK_ActivationLogs_Licenses")
                        .IsRequired();
                });

            modelBuilder.Entity("net_core_backend.Models.LicenseProducts", b =>
                {
                    b.HasOne("net_core_backend.Models.Licenses", "License")
                        .WithMany("LicenseProducts")
                        .HasForeignKey("LicenseId")
                        .HasConstraintName("FK_LicenseProducts_Licenses")
                        .IsRequired();

                    b.HasOne("net_core_backend.Models.Products", "Product")
                        .WithMany("LicenseProducts")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK_LicenseProducts_Products")
                        .IsRequired();
                });

            modelBuilder.Entity("net_core_backend.Models.Licenses", b =>
                {
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
                        .HasConstraintName("FK_RefreshTokens_Users")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
