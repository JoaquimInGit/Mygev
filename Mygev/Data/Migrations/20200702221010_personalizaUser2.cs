using Microsoft.EntityFrameworkCore.Migrations;

namespace Mygev.Migrations
{
    public partial class personalizaUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Utilizadores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Utilizadores");
        }
    }
}
