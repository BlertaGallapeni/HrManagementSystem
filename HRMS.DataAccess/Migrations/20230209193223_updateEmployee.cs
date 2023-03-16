using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.DataAccess.Migrations
{
    public partial class updateEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ApplicationUserId",
                table: "Employees",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_ApplicationUserId",
                table: "Employees",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_ApplicationUserId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ApplicationUserId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Employees");
        }
    }
}
