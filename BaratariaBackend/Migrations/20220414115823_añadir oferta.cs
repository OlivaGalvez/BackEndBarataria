using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BaratariaBackend.Migrations
{
    public partial class añadiroferta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OfertaId",
                table: "Documentos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Ofertas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "varchar(200)", nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "timestamp", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "timestamp", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Mostrar = table.Column<bool>(type: "boolean", nullable: true),
                    ImagenOriginal = table.Column<string>(type: "varchar(200)", nullable: true),
                    ImagenServidor = table.Column<string>(type: "varchar(200)", nullable: true),
                    ImagenPeso = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ofertas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_OfertaId",
                table: "Documentos",
                column: "OfertaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documentos_Ofertas_OfertaId",
                table: "Documentos",
                column: "OfertaId",
                principalTable: "Ofertas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documentos_Ofertas_OfertaId",
                table: "Documentos");

            migrationBuilder.DropTable(
                name: "Ofertas");

            migrationBuilder.DropIndex(
                name: "IX_Documentos_OfertaId",
                table: "Documentos");

            migrationBuilder.DropColumn(
                name: "OfertaId",
                table: "Documentos");
        }
    }
}
