using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_api.Migrations.Todo
{
    /// <inheritdoc />
    public partial class UpdateTodoItemSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "Secret",
                table: "TodoItems",
                newName: "Description");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TodoItems",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "TodoItems",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "TodoItems",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "TodoItems",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "TodoItems",
                newName: "Secret");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TodoItems",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
