using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace miniprojet.Migrations
{
    /// <inheritdoc />
    public partial class RenameLoginToUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Login",
                table: "Comptes",
                newName: "Username");

            migrationBuilder.RenameIndex(
                name: "IX_Comptes_Login",
                table: "Comptes",
                newName: "IX_Comptes_Username");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Comptes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Comptes");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Comptes",
                newName: "Login");

            migrationBuilder.RenameIndex(
                name: "IX_Comptes_Username",
                table: "Comptes",
                newName: "IX_Comptes_Login");
        }
    }
}
