using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASI_DotNet_API_V2.Migrations
{
    public partial class NewAnnotations1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "t_j_notation_not",
                newName: "t_j_notation_not",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "t_e_utilisateur_utl",
                newName: "t_e_utilisateur_utl",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "t_e_serie_ser",
                newName: "t_e_serie_ser",
                newSchema: "public");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "t_j_notation_not",
                schema: "public",
                newName: "t_j_notation_not");

            migrationBuilder.RenameTable(
                name: "t_e_utilisateur_utl",
                schema: "public",
                newName: "t_e_utilisateur_utl");

            migrationBuilder.RenameTable(
                name: "t_e_serie_ser",
                schema: "public",
                newName: "t_e_serie_ser");
        }
    }
}
