using Microsoft.EntityFrameworkCore.Migrations;

namespace net_core_backend.Migrations
{
    public partial class AddingMessageAndFigmaIdToActivationLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "active",
                table: "Licenses");

            migrationBuilder.RenameColumn(
                name: "successfull",
                table: "ActivationLogs",
                newName: "successful");

            migrationBuilder.AddColumn<string>(
                name: "figma_user_id",
                table: "ActivationLogs",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "message",
                table: "ActivationLogs",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "figma_user_id",
                table: "ActivationLogs");

            migrationBuilder.DropColumn(
                name: "message",
                table: "ActivationLogs");

            migrationBuilder.RenameColumn(
                name: "successful",
                table: "ActivationLogs",
                newName: "successfull");

            migrationBuilder.AddColumn<bool>(
                name: "active",
                table: "Licenses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
