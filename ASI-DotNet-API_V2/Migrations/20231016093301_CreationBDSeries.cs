using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ASI_DotNet_API_V2.Migrations
{
    public partial class CreationBDSeries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Serie",
                columns: table => new
                {
                    ser_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ser_titre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ser_resume = table.Column<string>(type: "TEXT", nullable: true),
                    ser_nbsaisons = table.Column<int>(type: "integer", nullable: true),
                    ser_nbepisodes = table.Column<int>(type: "integer", nullable: true),
                    ser_anneecreation = table.Column<int>(type: "integer", nullable: true),
                    ser_network = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Serie_ser_id", x => x.ser_id);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateur",
                columns: table => new
                {
                    utl_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    utl_nom = table.Column<string>(type: "varchar(50)", nullable: true),
                    utl_prenom = table.Column<string>(type: "varchar(50)", nullable: true),
                    utl_mobile = table.Column<string>(type: "char(10)", nullable: true),
                    utl_mail = table.Column<string>(type: "varchar(100)", nullable: false),
                    utl_pwd = table.Column<string>(type: "varchar(64)", nullable: false),
                    utl_rue = table.Column<string>(type: "varchar(200)", nullable: true),
                    utl_cp = table.Column<string>(type: "char(5)", nullable: true),
                    utl_ville = table.Column<string>(type: "varchar(50)", nullable: true),
                    utl_pays = table.Column<string>(type: "varchar(50)", nullable: true, defaultValue: "France"),
                    utl_latitude = table.Column<float>(type: "float", nullable: true),
                    utl_longitude = table.Column<float>(type: "float", nullable: true),
                    utl_datecreation = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateur_utl_id", x => x.utl_id);
                });

            migrationBuilder.CreateTable(
                name: "Notation",
                columns: table => new
                {
                    utl_id = table.Column<int>(type: "integer", nullable: false),
                    ser_id = table.Column<int>(type: "integer", nullable: false),
                    not_note = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notation_utl_id_ser_id", x => new { x.utl_id, x.ser_id });
                    table.ForeignKey(
                        name: "FK_Notation_Serie_ser_id",
                        column: x => x.ser_id,
                        principalTable: "Serie",
                        principalColumn: "ser_id");
                    table.ForeignKey(
                        name: "FK_Notation_Utilisateur_utl_id",
                        column: x => x.utl_id,
                        principalTable: "Utilisateur",
                        principalColumn: "utl_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notation_ser_id",
                table: "Notation",
                column: "ser_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notation");

            migrationBuilder.DropTable(
                name: "Serie");

            migrationBuilder.DropTable(
                name: "Utilisateur");
        }
    }
}
