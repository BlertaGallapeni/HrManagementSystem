using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.DataAccess.Migrations
{
    public partial class updateProjectTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameAl",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameAl",
                table: "Projects");
        }
    }
}
