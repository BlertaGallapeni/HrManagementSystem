using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.DataAccess.Migrations
{
    public partial class updatePriorityAndRateType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameAl",
                table: "RateTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameAl",
                table: "Priorities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameAl",
                table: "RateTypes");

            migrationBuilder.DropColumn(
                name: "NameAl",
                table: "Priorities");
        }
    }
}
