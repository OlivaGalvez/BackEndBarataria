using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BaratariaBackend.Migrations
{
    public partial class tablaasociacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AsociacionId",
                table: "Documentos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Asociacions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Texto = table.Column<string>(type: "varchar", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asociacions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_AsociacionId",
                table: "Documentos",
                column: "AsociacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documentos_Asociacions_AsociacionId",
                table: "Documentos",
                column: "AsociacionId",
                principalTable: "Asociacions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documentos_Asociacions_AsociacionId",
                table: "Documentos");

            migrationBuilder.DropTable(
                name: "Asociacions");

            migrationBuilder.DropIndex(
                name: "IX_Documentos_AsociacionId",
                table: "Documentos");

            migrationBuilder.DropColumn(
                name: "AsociacionId",
                table: "Documentos");
        }
    }
}
