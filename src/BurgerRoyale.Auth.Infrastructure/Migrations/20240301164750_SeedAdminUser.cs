using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BurgerRoyale.Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "Users",
                newName: "UserRole");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Cpf", "Name", "Email", "PasswordHash", "UserRole", "CreatedAt" },
                values: new object[,]
                {
                    { new Guid("7D562D53-763B-4A64-887D-8E07C153E13D"), "00000000000", "Admin", "admin@burgerroyale.com", "", 0, DateTime.UtcNow }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7D562D53-763B-4A64-887D-8E07C153E13D"));

            migrationBuilder.RenameColumn(
                name: "UserRole",
                table: "Users",
                newName: "UserType");
        }
    }
}
