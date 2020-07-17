using Microsoft.EntityFrameworkCore.Migrations;

namespace Mygev.Migrations
{
    public partial class relacaoContUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IDUser",
                table: "EventoConteudo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EventoConteudo_IDUser",
                table: "EventoConteudo",
                column: "IDUser");

            migrationBuilder.AddForeignKey(
                name: "FK_EventoConteudo_Utilizadores_IDUser",
                table: "EventoConteudo",
                column: "IDUser",
                principalTable: "Utilizadores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventoConteudo_Utilizadores_IDUser",
                table: "EventoConteudo");

            migrationBuilder.DropIndex(
                name: "IX_EventoConteudo_IDUser",
                table: "EventoConteudo");

            migrationBuilder.DropColumn(
                name: "IDUser",
                table: "EventoConteudo");
        }
    }
}
