using Microsoft.EntityFrameworkCore.Migrations;

namespace net_core_backend.Migrations
{
    public partial class removeFigmaIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "figma_user_id",
                table: "ActivationLogs");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "figma_user_id",
                table: "ActivationLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");


        }
    }
}
