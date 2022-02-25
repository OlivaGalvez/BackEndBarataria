using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BaratariaBackend.Migrations
{
    public partial class Addmásentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enlaces_Actividades_ActividadId",
                table: "Enlaces");

            migrationBuilder.AddColumn<int>(
                name: "SorteoId",
                table: "Socios",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Enlaces",
                type: "varchar",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ActividadId",
                table: "Enlaces",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Texto",
                table: "Actividades",
                type: "varchar",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(2000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaBaja",
                table: "Actividades",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AddColumn<int>(
                name: "IdTpActividad",
                table: "Actividades",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Enlaces_Actividades_ActividadId",
                table: "Enlaces",
                column: "ActividadId",
                principalTable: "Actividades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Socios_Sorteos_SorteoId",
                table: "Socios",
                column: "SorteoId",
                principalTable: "Sorteos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enlaces_Actividades_ActividadId",
                table: "Enlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_Socios_Sorteos_SorteoId",
                table: "Socios");

            migrationBuilder.DropTable(
                name: "EnlacesSorteo");

            migrationBuilder.DropTable(
                name: "SociosSorteoRlExcluidos");

            migrationBuilder.DropTable(
                name: "SociosSorteoRlGanadores");

            migrationBuilder.DropTable(
                name: "TpSorteos");

            migrationBuilder.DropTable(
                name: "Sorteos");

            migrationBuilder.DropIndex(
                name: "IX_Socios_SorteoId",
                table: "Socios");

            migrationBuilder.DropColumn(
                name: "SorteoId",
                table: "Socios");

            migrationBuilder.DropColumn(
                name: "IdTpActividad",
                table: "Actividades");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Enlaces",
                type: "varchar(2000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ActividadId",
                table: "Enlaces",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Texto",
                table: "Actividades",
                type: "varchar(2000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaBaja",
                table: "Actividades",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Enlaces_Actividades_ActividadId",
                table: "Enlaces",
                column: "ActividadId",
                principalTable: "Actividades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
