using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.DataAccess.Migrations
{
    public partial class addUserThumbnailTableToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserThumbnail",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserThumbnail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserThumbnail_UserThumbnail",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserThumbnail");
        }
    }
}
