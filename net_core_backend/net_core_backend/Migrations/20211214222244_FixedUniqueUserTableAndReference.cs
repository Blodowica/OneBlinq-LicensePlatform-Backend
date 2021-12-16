using Microsoft.EntityFrameworkCore.Migrations;

namespace net_core_backend.Migrations
{
    public partial class FixedUniqueUserTableAndReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivationLogs_UniqueUser",
                table: "ActivationLogs");

            migrationBuilder.DropTable(
                name: "UniqueUser");

            migrationBuilder.RenameColumn(
                name: "UniqueUserId",
                table: "ActivationLogs",
                newName: "unique_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_ActivationLogs_UniqueUserId",
                table: "ActivationLogs",
                newName: "IX_ActivationLogs_unique_user_id");

            migrationBuilder.CreateTable(
                name: "UniqueUsers",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    external_user_Id = table.Column<string>(nullable: false),
                    service = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniqueUsers", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ActivationLogs_UniqueUser",
                table: "ActivationLogs",
                column: "unique_user_id",
                principalTable: "UniqueUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivationLogs_UniqueUser",
                table: "ActivationLogs");

            migrationBuilder.DropTable(
                name: "UniqueUsers");

            migrationBuilder.RenameColumn(
                name: "unique_user_id",
                table: "ActivationLogs",
                newName: "UniqueUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ActivationLogs_unique_user_id",
                table: "ActivationLogs",
                newName: "IX_ActivationLogs_UniqueUserId");

            migrationBuilder.CreateTable(
                name: "UniqueUser",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userProductId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniqueUser", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ActivationLogs_UniqueUser",
                table: "ActivationLogs",
                column: "UniqueUserId",
                principalTable: "UniqueUser",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
