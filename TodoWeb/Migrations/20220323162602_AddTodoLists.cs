using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoWeb.Migrations
{
    public partial class AddTodoLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Users_CreatedById",
                table: "Todos");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Todos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TodoListId",
                table: "Todos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TodoLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoLists_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoauthorUserTodoList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ListId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoauthorUserTodoList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoauthorUserTodoList_TodoLists_ListId",
                        column: x => x.ListId,
                        principalTable: "TodoLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CoauthorUserTodoList_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todos_TodoListId",
                table: "Todos",
                column: "TodoListId");

            migrationBuilder.CreateIndex(
                name: "IX_CoauthorUserTodoList_ListId",
                table: "CoauthorUserTodoList",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_CoauthorUserTodoList_UserId",
                table: "CoauthorUserTodoList",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoLists_CreatedById",
                table: "TodoLists",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TodoLists_TodoListId",
                table: "Todos",
                column: "TodoListId",
                principalTable: "TodoLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Users_CreatedById",
                table: "Todos",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TodoLists_TodoListId",
                table: "Todos");

            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Users_CreatedById",
                table: "Todos");

            migrationBuilder.DropTable(
                name: "CoauthorUserTodoList");

            migrationBuilder.DropTable(
                name: "TodoLists");

            migrationBuilder.DropIndex(
                name: "IX_Todos_TodoListId",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "TodoListId",
                table: "Todos");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Todos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Users_CreatedById",
                table: "Todos",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
