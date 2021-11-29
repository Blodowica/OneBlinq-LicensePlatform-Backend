using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace net_core_backend.Migrations
{
    public partial class AddingAdditionalInfoForUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "Users",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "date_of_birth",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "Users",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "country",
                table: "Users",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "postal_code",
                table: "Users",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "date_of_birth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "city",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "country",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "postal_code",
                table: "Users");
        }
    }
}
