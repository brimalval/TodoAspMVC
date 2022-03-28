using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoWeb.Migrations
{
    public partial class UpdateTodoListNormalizeAuthors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoLists_Users_CreatedById",
                table: "TodoLists");

            migrationBuilder.DropTable(
                name: "TodoListCoauthorships");

            migrationBuilder.DropIndex(
                name: "IX_TodoLists_CreatedById",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "TodoLists");

            migrationBuilder.CreateTable(
                name: "TodoListUser",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "int", nullable: false),
                    TodoListsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoListUser", x => new { x.AuthorsId, x.TodoListsId });
                    table.ForeignKey(
                        name: "FK_TodoListUser_TodoLists_TodoListsId",
                        column: x => x.TodoListsId,
                        principalTable: "TodoLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TodoListUser_Users_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoListUser_TodoListsId",
                table: "TodoListUser",
                column: "TodoListsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoListUser");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "TodoLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TodoListCoauthorships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoListCoauthorships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoListCoauthorships_TodoLists_ListId",
                        column: x => x.ListId,
                        principalTable: "TodoLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TodoListCoauthorships_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoLists_CreatedById",
                table: "TodoLists",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TodoListCoauthorships_ListId",
                table: "TodoListCoauthorships",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoListCoauthorships_UserId",
                table: "TodoListCoauthorships",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoLists_Users_CreatedById",
                table: "TodoLists",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
