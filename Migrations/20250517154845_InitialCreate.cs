using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace miniprojet.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comptes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "User")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comptes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locataires",
                columns: table => new
                {
                    IdLoc = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Prénom = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locataires", x => x.IdLoc);
                });

            migrationBuilder.CreateTable(
                name: "Propriétaires",
                columns: table => new
                {
                    IdProp = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Prénom = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propriétaires", x => x.IdProp);
                });

            migrationBuilder.CreateTable(
                name: "Appartements",
                columns: table => new
                {
                    NumApp = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdProp = table.Column<int>(type: "INTEGER", nullable: false),
                    Localite = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    NbrPièces = table.Column<int>(type: "INTEGER", nullable: false),
                    Valeur = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appartements", x => x.NumApp);
                    table.ForeignKey(
                        name: "FK_Appartements_Propriétaires_IdProp",
                        column: x => x.IdProp,
                        principalTable: "Propriétaires",
                        principalColumn: "IdProp",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    IdLoc = table.Column<int>(type: "INTEGER", nullable: false),
                    NumApp = table.Column<int>(type: "INTEGER", nullable: false),
                    DatLoc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NbrMois = table.Column<int>(type: "INTEGER", nullable: false),
                    Montant = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.IdLoc);
                    table.ForeignKey(
                        name: "FK_Locations_Appartements_NumApp",
                        column: x => x.NumApp,
                        principalTable: "Appartements",
                        principalColumn: "NumApp",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Locations_Locataires_IdLoc",
                        column: x => x.IdLoc,
                        principalTable: "Locataires",
                        principalColumn: "IdLoc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appartements_IdProp",
                table: "Appartements",
                column: "IdProp");

            migrationBuilder.CreateIndex(
                name: "IX_Comptes_Login",
                table: "Comptes",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_NumApp",
                table: "Locations",
                column: "NumApp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comptes");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Appartements");

            migrationBuilder.DropTable(
                name: "Locataires");

            migrationBuilder.DropTable(
                name: "Propriétaires");
        }
    }
}
