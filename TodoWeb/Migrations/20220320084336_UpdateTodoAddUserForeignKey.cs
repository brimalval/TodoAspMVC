using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoWeb.Migrations
{
    public partial class UpdateTodoAddUserForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Todos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Todos_CreatedById",
                table: "Todos",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Users_CreatedById",
                table: "Todos",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Users_CreatedById",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_CreatedById",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Todos");
        }
    }
}
