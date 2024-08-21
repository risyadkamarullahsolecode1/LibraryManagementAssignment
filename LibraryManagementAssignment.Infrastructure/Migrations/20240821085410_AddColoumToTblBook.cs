using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementAssignment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColoumToTblBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TotalBook",
                table: "Books",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeleteStamp",
                table: "Books",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DeleteStatus",
                table: "Books",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteStamp",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "DeleteStatus",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "TotalBook",
                table: "Books",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
