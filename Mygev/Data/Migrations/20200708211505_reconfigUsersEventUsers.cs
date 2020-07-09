using Microsoft.EntityFrameworkCore.Migrations;

namespace Mygev.Migrations
{
    public partial class reconfigUsersEventUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventoUtilizadores",
                table: "EventoUtilizadores");

            migrationBuilder.DropColumn(
                name: "IDEU",
                table: "EventoUtilizadores");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Utilizadores",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Utilizadores",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Localidade",
                table: "Utilizadores",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Permissao",
                table: "EventoUtilizadores",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "EventoUtilizadores",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventoUtilizadores",
                table: "EventoUtilizadores",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventoUtilizadores",
                table: "EventoUtilizadores");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "Localidade",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "EventoUtilizadores");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Utilizadores",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "Permissao",
                table: "EventoUtilizadores",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IDEU",
                table: "EventoUtilizadores",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventoUtilizadores",
                table: "EventoUtilizadores",
                column: "IDEU");
        }
    }
}
