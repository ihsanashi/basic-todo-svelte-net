using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_api.Migrations.Todo
{
    /// <inheritdoc />
    public partial class AddUserIdAsForeignKeyToTodoItems : Migration
    {
        /// <inheritdoc />
        // Add the UserId column to the existing TodoItems table
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TodoItems",
                type: "varchar(450)",
                nullable: false
            );

            // Create the foreign key constraint referencing the AspNetUsers table
            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_AspNetUsers_UserId",
                table: "TodoItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_AspNetUsers_UserId",
                table: "TodoItems");

            // Remove the UserId column
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TodoItems");
        }
    }
}
