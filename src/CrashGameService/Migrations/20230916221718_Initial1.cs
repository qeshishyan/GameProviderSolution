using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrashGameService.Migrations
{
    public partial class Initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_GameRounds_CurrentRoundId",
                table: "GameSessions");

            migrationBuilder.DropIndex(
                name: "IX_CashOuts_BetId",
                table: "CashOuts");

            migrationBuilder.AddColumn<int>(
                name: "BetId1",
                table: "CashOuts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashOuts_BetId",
                table: "CashOuts",
                column: "BetId",
                unique: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_GameRounds_CurrentRoundId",
                table: "GameSessions",
                column: "CurrentRoundId",
                principalTable: "GameRounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashOuts_Bets_BetId1",
                table: "CashOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_GameRounds_CurrentRoundId",
                table: "GameSessions");

            migrationBuilder.DropIndex(
                name: "IX_CashOuts_BetId",
                table: "CashOuts");

            migrationBuilder.DropIndex(
                name: "IX_CashOuts_BetId1",
                table: "CashOuts");

            migrationBuilder.DropColumn(
                name: "BetId1",
                table: "CashOuts");

            migrationBuilder.CreateIndex(
                name: "IX_CashOuts_BetId",
                table: "CashOuts",
                column: "BetId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_GameRounds_CurrentRoundId",
                table: "GameSessions",
                column: "CurrentRoundId",
                principalTable: "GameRounds",
                principalColumn: "Id");
        }
    }
}
