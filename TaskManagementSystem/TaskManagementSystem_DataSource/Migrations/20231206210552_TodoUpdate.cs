using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementSystem_DataSource.Migrations
{
    public partial class TodoUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "TodoLists",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<int>(
                name: "PriorityLevel",
                table: "TodoLists",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "TodoLists",
                newName: "Created");

            migrationBuilder.AlterColumn<string>(
                name: "PriorityLevel",
                table: "TodoLists",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
