using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BurgerRoyale.Auth.Infrastructure.Migrations;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public partial class SeedUsers : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("e36728b8-95dd-4e78-9626-666c37eacfe2"));

        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Id", "Cpf", "CreatedAt", "Email", "Name", "PasswordHash", "UpdatedAt", "UserRole" },
            values: new object[,]
            {
                { new Guid("392d716a-20af-48a7-b8ef-e443dd4d8134"), "11111111111", new DateTime(2024, 3, 8, 18, 28, 48, 875, DateTimeKind.Utc).AddTicks(4849), "customer@burgerroyale.com", "Customer", "$2a$11$mDJa/xLGCCAYzxhDpcmYve8NpaBqMeCMMfVbB9NpGqg/SpaRv3gJq", null, 1 },
                { new Guid("c39285ad-fd4d-49d7-a886-96bb62507212"), "00000000000", new DateTime(2024, 3, 8, 18, 28, 48, 875, DateTimeKind.Utc).AddTicks(3810), "admin@burgerroyale.com", "Admin", "$2a$11$C99.K9/gfTc0RqR8XYAiu.T3BG/GvWgOt2oggKkyivz9dGpZPwpEy", null, 0 },
                { new Guid("c41144a9-7bc0-4444-a041-a1cdbba46d90"), "22222222222", new DateTime(2024, 3, 8, 18, 28, 48, 875, DateTimeKind.Utc).AddTicks(5246), "employee@burgerroyale.com", "Employee", "$2a$11$hIwenwL9SKoqgwWt0EOQgukwjCDP1tVuij0lMtHk9nGSwnIVbzbM2", null, 2 }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("392d716a-20af-48a7-b8ef-e443dd4d8134"));

        migrationBuilder.DeleteData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("c39285ad-fd4d-49d7-a886-96bb62507212"));

        migrationBuilder.DeleteData(
            table: "Users",
            keyColumn: "Id",
            keyValue: new Guid("c41144a9-7bc0-4444-a041-a1cdbba46d90"));

        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Id", "Cpf", "CreatedAt", "Email", "Name", "PasswordHash", "UpdatedAt", "UserRole" },
            values: new object[] { new Guid("e36728b8-95dd-4e78-9626-666c37eacfe2"), "00000000000", new DateTime(2024, 3, 5, 1, 10, 21, 168, DateTimeKind.Utc).AddTicks(2822), "admin@burgerroyale.com", "Admin", "$2a$11$Hm3GUkwCnSTCFwqT1ntowe/C/rvm2lery.SP3tUVe0.qdMyknR5PG", null, 0 });
    }
}
