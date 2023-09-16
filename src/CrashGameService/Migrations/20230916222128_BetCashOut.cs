using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrashGameService.Migrations
{
    public partial class BetCashOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashOuts_Bets_BetId1",
                table: "CashOuts");

            migrationBuilder.DropIndex(
                name: "IX_CashOuts_BetId1",
                table: "CashOuts");

            migrationBuilder.DropColumn(
                name: "BetId1",
                table: "CashOuts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BetId1",
                table: "CashOuts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashOuts_BetId1",
                table: "CashOuts",
                column: "BetId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CashOuts_Bets_BetId1",
                table: "CashOuts",
                column: "BetId1",
                principalTable: "Bets",
                principalColumn: "Id");
        }
    }
}
