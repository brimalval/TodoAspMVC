using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoWeb.Migrations
{
    public partial class UpdateTodoListAddDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoauthorUserTodoList_TodoLists_ListId",
                table: "CoauthorUserTodoList");

            migrationBuilder.DropForeignKey(
                name: "FK_CoauthorUserTodoList_Users_UserId",
                table: "CoauthorUserTodoList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CoauthorUserTodoList",
                table: "CoauthorUserTodoList");

            migrationBuilder.RenameTable(
                name: "CoauthorUserTodoList",
                newName: "TodoListCoauthorships");

            migrationBuilder.RenameIndex(
                name: "IX_CoauthorUserTodoList_UserId",
                table: "TodoListCoauthorships",
                newName: "IX_TodoListCoauthorships_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CoauthorUserTodoList_ListId",
                table: "TodoListCoauthorships",
                newName: "IX_TodoListCoauthorships_ListId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TodoLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoListCoauthorships",
                table: "TodoListCoauthorships",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoListCoauthorships_TodoLists_ListId",
                table: "TodoListCoauthorships",
                column: "ListId",
                principalTable: "TodoLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoListCoauthorships_Users_UserId",
                table: "TodoListCoauthorships",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoListCoauthorships_TodoLists_ListId",
                table: "TodoListCoauthorships");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoListCoauthorships_Users_UserId",
                table: "TodoListCoauthorships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoListCoauthorships",
                table: "TodoListCoauthorships");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TodoLists");

            migrationBuilder.RenameTable(
                name: "TodoListCoauthorships",
                newName: "CoauthorUserTodoList");

            migrationBuilder.RenameIndex(
                name: "IX_TodoListCoauthorships_UserId",
                table: "CoauthorUserTodoList",
                newName: "IX_CoauthorUserTodoList_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TodoListCoauthorships_ListId",
                table: "CoauthorUserTodoList",
                newName: "IX_CoauthorUserTodoList_ListId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CoauthorUserTodoList",
                table: "CoauthorUserTodoList",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CoauthorUserTodoList_TodoLists_ListId",
                table: "CoauthorUserTodoList",
                column: "ListId",
                principalTable: "TodoLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CoauthorUserTodoList_Users_UserId",
                table: "CoauthorUserTodoList",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
