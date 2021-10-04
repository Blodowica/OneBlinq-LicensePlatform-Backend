using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace net_core_backend.Migrations
{
    public partial class RemovedLicenseProductsTableAndUpdatedConnectedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenseProducts");

            migrationBuilder.DropColumn(
                name: "currency",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "price",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "recurrance",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "gumroad_id",
                table: "Licenses");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "Users",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "Users",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "max_uses",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "variant_name",
                table: "Products",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "expires_at",
                table: "Licenses",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "currency",
                table: "Licenses",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gumroad_sale_id",
                table: "Licenses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gumroad_subscription_id",
                table: "Licenses",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "price",
                table: "Licenses",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Licenses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "recurrence",
                table: "Licenses",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_ProductId",
                table: "Licenses",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Licenses_Products",
                table: "Licenses",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Licenses_Products",
                table: "Licenses");

            migrationBuilder.DropIndex(
                name: "IX_Licenses_ProductId",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "max_uses",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "variant_name",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "currency",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "gumroad_sale_id",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "gumroad_subscription_id",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "price",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "recurrence",
                table: "Licenses");

            migrationBuilder.AlterColumn<string>(
                name: "last_name",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "first_name",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "currency",
                table: "Products",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "price",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "recurrance",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "expires_at",
                table: "Licenses",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gumroad_id",
                table: "Licenses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LicenseProducts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicenseId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseProducts", x => x.id);
                    table.ForeignKey(
                        name: "FK_LicenseProducts_Licenses",
                        column: x => x.LicenseId,
                        principalTable: "Licenses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseProducts_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseProducts_LicenseId",
                table: "LicenseProducts",
                column: "LicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseProducts_ProductId",
                table: "LicenseProducts",
                column: "ProductId");
        }
    }
}
