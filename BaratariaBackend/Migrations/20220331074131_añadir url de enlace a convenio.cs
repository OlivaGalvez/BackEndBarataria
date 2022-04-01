using Microsoft.EntityFrameworkCore.Migrations;

namespace BaratariaBackend.Migrations
{
    public partial class añadirurldeenlaceaconvenio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Convenios",
                type: "varchar",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Convenios");
        }
    }
}
