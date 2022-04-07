using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BaratariaBackend.Migrations
{
    public partial class prueba : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actividades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "varchar(200)", nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "timestamp", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "timestamp", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Mostrar = table.Column<bool>(type: "boolean", nullable: true),
                    Texto = table.Column<string>(type: "varchar", nullable: true),
                    ImagenOriginal = table.Column<string>(type: "varchar(200)", nullable: true),
                    ImagenServidor = table.Column<string>(type: "varchar(200)", nullable: true),
                    ImagenPeso = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actividades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Enlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "varchar(200)", nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Mostrar = table.Column<bool>(type: "boolean", nullable: true),
                    ImagenOriginal = table.Column<string>(type: "varchar", nullable: true),
                    ImagenServidor = table.Column<string>(type: "varchar(200)", nullable: true),
                    ImagenPeso = table.Column<long>(type: "bigint", nullable: true),
                    Url = table.Column<string>(type: "varchar", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enlaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActividadId = table.Column<int>(type: "integer", nullable: true),
                    SocioId = table.Column<int>(type: "integer", nullable: true),
                    Nombre = table.Column<string>(type: "varchar(200)", nullable: true),
                    Original = table.Column<string>(type: "varchar(200)", nullable: true),
                    Servidor = table.Column<string>(type: "varchar(500)", nullable: true),
                    Url = table.Column<string>(type: "varchar(500)", nullable: true),
                    Tamanio = table.Column<string>(type: "varchar(500)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Privado = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documentos_Actividades_ActividadId",
                        column: x => x.ActividadId,
                        principalTable: "Actividades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EnlacesActividad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActividadId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(200)", nullable: true),
                    Url = table.Column<string>(type: "varchar", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnlacesActividad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnlacesActividad_Actividades_ActividadId",
                        column: x => x.ActividadId,
                        principalTable: "Actividades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TpDocumentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "varchar(200)", nullable: true),
                    DocumentoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TpDocumentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TpDocumentos_Documentos_DocumentoId",
                        column: x => x.DocumentoId,
                        principalTable: "Documentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_ActividadId",
                table: "Documentos",
                column: "ActividadId");

            migrationBuilder.CreateIndex(
                name: "IX_EnlacesActividad_ActividadId",
                table: "EnlacesActividad",
                column: "ActividadId");

            migrationBuilder.CreateIndex(
                name: "IX_TpDocumentos_DocumentoId",
                table: "TpDocumentos",
                column: "DocumentoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enlaces");

            migrationBuilder.DropTable(
                name: "EnlacesActividad");

            migrationBuilder.DropTable(
                name: "TpDocumentos");

            migrationBuilder.DropTable(
                name: "Documentos");

            migrationBuilder.DropTable(
                name: "Actividades");
        }
    }
}
