using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BaratariaBackend.Migrations
{
    public partial class AddentitiesDeporteyEnlacesDeporte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeporteId",
                table: "Enlaces",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Deportes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "varchar(200)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "date", nullable: false),
                    Hora = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Texto = table.Column<string>(type: "varchar", nullable: true),
                    ImagenOriginal = table.Column<string>(type: "varchar(200)", nullable: true),
                    ImagenServidor = table.Column<string>(type: "varchar(200)", nullable: true),
                    ImagenPeso = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deportes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnlacesDeporte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeporteId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(200)", nullable: true),
                    Url = table.Column<string>(type: "varchar", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnlacesDeporte", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Imagenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "varchar(200)", nullable: true),
                    Url = table.Column<string>(type: "varchar", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagenes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enlaces_DeporteId",
                table: "Enlaces",
                column: "DeporteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enlaces_Deportes_DeporteId",
                table: "Enlaces",
                column: "DeporteId",
                principalTable: "Deportes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enlaces_Deportes_DeporteId",
                table: "Enlaces");

            migrationBuilder.DropTable(
                name: "Deportes");

            migrationBuilder.DropTable(
                name: "EnlacesDeporte");

            migrationBuilder.DropTable(
                name: "Imagenes");

            migrationBuilder.DropIndex(
                name: "IX_Enlaces_DeporteId",
                table: "Enlaces");

            migrationBuilder.DropColumn(
                name: "DeporteId",
                table: "Enlaces");
        }
    }
}
