using Microsoft.EntityFrameworkCore.Migrations;

namespace net_core_backend.Migrations
{
    public partial class AddedActivatablePluginsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivateablePlugins",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    plugin = table.Column<string>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivateablePlugins", x => x.id);
                    table.ForeignKey(
                        name: "FK_ActivateablePlugins_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivateablePlugins_ProductId",
                table: "ActivateablePlugins",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivateablePlugins");
        }
    }
}
