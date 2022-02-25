using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BaratariaBackend.Migrations
{
    public partial class AddInicial : Migration
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
                    Fecha = table.Column<DateTime>(type: "date", nullable: false),
                    Hora = table.Column<DateTime>(type: "timestamp", nullable: false),
                    FechaBaja = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Mostrar = table.Column<bool>(type: "boolean", nullable: true),
                    Texto = table.Column<string>(type: "varchar(2000)", nullable: true),
                    ImagenOriginal = table.Column<string>(type: "varchar(200)", nullable: true),
                    ImagenServidor = table.Column<string>(type: "varchar(200)", nullable: true),
                    ImagenPeso = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actividades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Socios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActividadId = table.Column<int>(type: "integer", nullable: false),
                    SocioId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(200)", nullable: true),
                    Descripcion = table.Column<string>(type: "varchar(100)", nullable: true),
                    Original = table.Column<string>(type: "varchar(200)", nullable: true),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enlaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActividadId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(200)", nullable: true),
                    Url = table.Column<string>(type: "varchar(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enlaces_Actividades_ActividadId",
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
                name: "IX_Enlaces_ActividadId",
                table: "Enlaces",
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
                name: "Socios");

            migrationBuilder.DropTable(
                name: "TpDocumentos");

            migrationBuilder.DropTable(
                name: "Documentos");

            migrationBuilder.DropTable(
                name: "Actividades");
        }
    }
}
