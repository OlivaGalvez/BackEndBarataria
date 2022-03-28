using Microsoft.EntityFrameworkCore.Migrations;

namespace BaratariaBackend.Migrations
{
    public partial class rlactividadenlace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EnlacesActividad_ActividadId",
                table: "EnlacesActividad",
                column: "ActividadId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnlacesActividad_Actividades_ActividadId",
                table: "EnlacesActividad",
                column: "ActividadId",
                principalTable: "Actividades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnlacesActividad_Actividades_ActividadId",
                table: "EnlacesActividad");

            migrationBuilder.DropIndex(
                name: "IX_EnlacesActividad_ActividadId",
                table: "EnlacesActividad");
        }
    }
}
