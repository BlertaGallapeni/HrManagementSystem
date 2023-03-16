using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.DataAccess.Migrations
{
    public partial class addForeignKeyForStatusToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_StatusId",
                table: "Clients",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Status_StatusId",
                table: "Clients",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Status_StatusId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_StatusId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Clients");
        }
    }
}
