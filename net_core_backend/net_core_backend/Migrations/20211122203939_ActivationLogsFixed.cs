using Microsoft.EntityFrameworkCore.Migrations;

namespace net_core_backend.Migrations
{
    public partial class ActivationLogsFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LicenseId",
                table: "ActivationLogs",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LicenseId",
                table: "ActivationLogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
