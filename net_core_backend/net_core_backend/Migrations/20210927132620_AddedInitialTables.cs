using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace net_core_backend.Migrations
{
    public partial class AddedInitialTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hashedPassword",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Users",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "gumroad_id",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "Users",
                maxLength: 250,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_at = table.Column<DateTime>(nullable: false),
                    expires_at = table.Column<DateTime>(nullable: false),
                    purchase_location = table.Column<string>(maxLength: 100, nullable: true),
                    active = table.Column<bool>(nullable: false),
                    gumroad_id = table.Column<string>(nullable: true),
                    license_key = table.Column<string>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.id);
                    table.ForeignKey(
                        name: "FK_Licenses_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_name = table.Column<string>(maxLength: 100, nullable: false),
                    price = table.Column<int>(nullable: false),
                    currency = table.Column<string>(maxLength: 10, nullable: true),
                    active = table.Column<bool>(nullable: false),
                    recurrance = table.Column<string>(maxLength: 50, nullable: false),
                    gumroad_id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    token = table.Column<string>(nullable: false),
                    expires_at = table.Column<DateTime>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    revoked_at = table.Column<DateTime>(nullable: false),
                    active = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActivationLogs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_at = table.Column<DateTime>(nullable: false),
                    successfull = table.Column<bool>(nullable: false),
                    LicenseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivationLogs", x => x.id);
                    table.ForeignKey(
                        name: "FK_ActivationLogs_Licenses",
                        column: x => x.LicenseId,
                        principalTable: "Licenses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LicenseProducts",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicenseId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
               name: "UniqueIds",
               columns: table => new
               {
                   id = table.Column<int>(nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   userProductId = table.Column<string>(maxLength: 100, nullable: true),
                   product = table.Column<string>(maxLength: 100, nullable: false),
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_UniqueIds", x => x.id);
                   table.ForeignKey(
                       name: "FK_UniqueIds_Users",
                       column: x => x.userProductId,
                       principalTable: "Users",
                       principalColumn: "id",
                       onDelete: ReferentialAction.Restrict);
               });




            migrationBuilder.CreateIndex(
                name: "IX_ActivationLogs_LicenseId",
                table: "ActivationLogs",
                column: "LicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseProducts_LicenseId",
                table: "LicenseProducts",
                column: "LicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseProducts_ProductId",
                table: "LicenseProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_UserId",
                table: "Licenses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivationLogs");

            migrationBuilder.DropTable(
                name: "LicenseProducts");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropColumn(
                name: "gumroad_id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "password",
                table: "Users");

            migrationBuilder.DropTable(
            name: "UniqueIds");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "hashedPassword",
                table: "Users",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }
    }
}
