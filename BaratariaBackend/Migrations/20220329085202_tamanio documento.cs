using Microsoft.EntityFrameworkCore.Migrations;

namespace BaratariaBackend.Migrations
{
    public partial class tamaniodocumento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tamanio",
                table: "Documentos",
                type: "varchar(500)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tamanio",
                table: "Documentos");
        }
    }
}
