using Microsoft.EntityFrameworkCore.Migrations;

namespace Mygev.Migrations
{
    public partial class passEvento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "passEvento",
                table: "Evento",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "passEvento",
                table: "Evento");
        }
    }
}
