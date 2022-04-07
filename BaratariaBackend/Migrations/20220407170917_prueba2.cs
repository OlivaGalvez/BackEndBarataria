using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BaratariaBackend.Migrations
{
    public partial class prueba2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "EnlacesSorteo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SorteoId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(200)", nullable: true),
                    Url = table.Column<string>(type: "varchar", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnlacesSorteo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SociosSorteoRlExcluidos",
                columns: table => new
                {
                    IdSocio = table.Column<int>(type: "integer", nullable: false),
                    IdSorteo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SociosSorteoRlExcluidos", x => new { x.IdSocio, x.IdSorteo });
                });

            migrationBuilder.CreateTable(
                name: "Sorteos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdTpSorteo = table.Column<int>(type: "integer", nullable: false),
                    Titulo = table.Column<string>(type: "varchar(200)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "date", nullable: false),
                    Hora = table.Column<DateTime>(type: "timestamp", nullable: false),
                    FechaBaja = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Mostrar = table.Column<bool>(type: "boolean", nullable: true),
                    Texto = table.Column<string>(type: "varchar", nullable: true),
                    ImagenOriginal = table.Column<string>(type: "varchar(200)", nullable: true),
                    ImagenServidor = table.Column<string>(type: "varchar(200)", nullable: true),
                    ImagenPeso = table.Column<long>(type: "bigint", nullable: false),
                    FechaSorteo = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Excluyente = table.Column<bool>(type: "boolean", nullable: true),
                    DiasRepeticion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sorteos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TpSorteos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TpSorteos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Socios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "varchar(50)", nullable: true),
                    SorteoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Socios_Sorteos_SorteoId",
                        column: x => x.SorteoId,
                        principalTable: "Sorteos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SociosSorteoRlGanadores",
                columns: table => new
                {
                    IdSocio = table.Column<int>(type: "integer", nullable: false),
                    IdSorteo = table.Column<int>(type: "integer", nullable: false),
                    SorteoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SociosSorteoRlGanadores", x => new { x.IdSocio, x.IdSorteo });
                    table.ForeignKey(
                        name: "FK_SociosSorteoRlGanadores_Sorteos_SorteoId",
                        column: x => x.SorteoId,
                        principalTable: "Sorteos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Socios_SorteoId",
                table: "Socios",
                column: "SorteoId");

            migrationBuilder.CreateIndex(
                name: "IX_SociosSorteoRlGanadores_SorteoId",
                table: "SociosSorteoRlGanadores",
                column: "SorteoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deportes");

            migrationBuilder.DropTable(
                name: "EnlacesDeporte");

            migrationBuilder.DropTable(
                name: "EnlacesSorteo");

            migrationBuilder.DropTable(
                name: "Socios");

            migrationBuilder.DropTable(
                name: "SociosSorteoRlExcluidos");

            migrationBuilder.DropTable(
                name: "SociosSorteoRlGanadores");

            migrationBuilder.DropTable(
                name: "TpSorteos");

            migrationBuilder.DropTable(
                name: "Sorteos");
        }
    }
}
