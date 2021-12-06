using Microsoft.EntityFrameworkCore.Migrations;

namespace net_core_backend.Migrations
{
    public partial class UniqueIdTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
