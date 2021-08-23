using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Usadosbr.Contas.Migrations.Migrations
{
    public partial class AdicionaAuditoriaModulo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auditoria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataOperacao = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Operacao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Entidade = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Tabela = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Chaves = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValoresOriginais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValoresNovos = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modulos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modulos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_Tabela",
                table: "Auditoria",
                column: "Tabela");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auditoria");

            migrationBuilder.DropTable(
                name: "Modulos");
        }
    }
}
