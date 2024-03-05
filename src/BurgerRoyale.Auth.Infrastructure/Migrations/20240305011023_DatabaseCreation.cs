using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BurgerRoyale.Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRole = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Cpf", "CreatedAt", "Email", "Name", "PasswordHash", "UpdatedAt", "UserRole" },
                values: new object[] { new Guid("e36728b8-95dd-4e78-9626-666c37eacfe2"), "00000000000", new DateTime(2024, 3, 5, 1, 10, 21, 168, DateTimeKind.Utc).AddTicks(2822), "admin@burgerroyale.com", "Admin", "$2a$11$Hm3GUkwCnSTCFwqT1ntowe/C/rvm2lery.SP3tUVe0.qdMyknR5PG", null, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
