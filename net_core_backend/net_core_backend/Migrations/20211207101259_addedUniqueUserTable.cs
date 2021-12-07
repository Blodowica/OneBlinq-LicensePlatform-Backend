using Microsoft.EntityFrameworkCore.Migrations;

namespace net_core_backend.Migrations
{
    public partial class addedUniqueUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UniqueUserId",
                table: "ActivationLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UniqueUser",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userProductId = table.Column<string>(nullable: false),
                    product = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniqueUser", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivationLogs_UniqueUserId",
                table: "ActivationLogs",
                column: "UniqueUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivationLogs_UniqueUser",
                table: "ActivationLogs",
                column: "UniqueUserId",
                principalTable: "UniqueUser",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivationLogs_UniqueUser",
                table: "ActivationLogs");

            migrationBuilder.DropTable(
                name: "UniqueUser");

            migrationBuilder.DropIndex(
                name: "IX_ActivationLogs_UniqueUserId",
                table: "ActivationLogs");

            migrationBuilder.DropColumn(
                name: "UniqueUserId",
                table: "ActivationLogs");
        }
    }
}
