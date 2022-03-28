using Microsoft.EntityFrameworkCore.Migrations;

namespace BaratariaBackend.Migrations
{
    public partial class cambioentitydocumento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documentos_Actividades_ActividadId",
                table: "Documentos");

            migrationBuilder.AlterColumn<int>(
                name: "SocioId",
                table: "Documentos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ActividadId",
                table: "Documentos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Documentos_Actividades_ActividadId",
                table: "Documentos",
                column: "ActividadId",
                principalTable: "Actividades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documentos_Actividades_ActividadId",
                table: "Documentos");

            migrationBuilder.AlterColumn<int>(
                name: "SocioId",
                table: "Documentos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ActividadId",
                table: "Documentos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Documentos_Actividades_ActividadId",
                table: "Documentos",
                column: "ActividadId",
                principalTable: "Actividades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
