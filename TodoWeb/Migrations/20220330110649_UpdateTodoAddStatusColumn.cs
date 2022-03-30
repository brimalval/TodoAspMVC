using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoWeb.Migrations
{
    public partial class UpdateTodoAddStatusColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Todos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Todos_StatusId",
                table: "Todos",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Statuses_StatusId",
                table: "Todos",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Statuses_StatusId",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_StatusId",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Todos");
        }
    }
}
