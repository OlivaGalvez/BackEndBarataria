using Microsoft.EntityFrameworkCore.Migrations;

namespace BaratariaBackend.Migrations
{
    public partial class cambioentitydocumento2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Documentos");

            migrationBuilder.AddColumn<string>(
                name: "Servidor",
                table: "Documentos",
                type: "varchar(500)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Servidor",
                table: "Documentos");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Documentos",
                type: "varchar(100)",
                nullable: true);
        }
    }
}
