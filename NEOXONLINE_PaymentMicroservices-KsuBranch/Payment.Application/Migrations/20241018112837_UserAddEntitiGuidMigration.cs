using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payment.Application.Migrations
{
    /// <inheritdoc />
    public partial class UserAddEntitiGuidMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNamber",
                table: "User",
                newName: "PhoneNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "User",
                newName: "PhoneNamber");
        }
    }
}
