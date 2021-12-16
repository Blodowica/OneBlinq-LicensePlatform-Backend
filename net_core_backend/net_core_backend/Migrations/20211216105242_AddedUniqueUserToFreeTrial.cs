using Microsoft.EntityFrameworkCore.Migrations;

namespace net_core_backend.Migrations
{
    public partial class AddedUniqueUserToFreeTrial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "figma_user_id",
                table: "FreeTrials");

            migrationBuilder.AddColumn<int>(
                name: "unique_user_id",
                table: "FreeTrials",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FreeTrials_unique_user_id",
                table: "FreeTrials",
                column: "unique_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_FreeTrials_UniqueUser",
                table: "FreeTrials",
                column: "unique_user_id",
                principalTable: "UniqueUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FreeTrials_UniqueUser",
                table: "FreeTrials");

            migrationBuilder.DropIndex(
                name: "IX_FreeTrials_unique_user_id",
                table: "FreeTrials");

            migrationBuilder.DropColumn(
                name: "unique_user_id",
                table: "FreeTrials");

            migrationBuilder.AddColumn<string>(
                name: "figma_user_id",
                table: "FreeTrials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
