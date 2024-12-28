using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_api.Migrations.Todo
{
    /// <inheritdoc />
    public partial class AddSoftDeletionToTodoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TodoItems",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "TodoItems",
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TodoItems"
            );

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "TodoItems"
            );
        }
    }
}
