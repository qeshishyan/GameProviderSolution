using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CrashGameService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GameRoundId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    BetDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Win = table.Column<bool>(type: "boolean", nullable: false),
                    Multiplier = table.Column<double>(type: "double precision", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashOuts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BetId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Multiplier = table.Column<double>(type: "double precision", nullable: false),
                    DateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashOuts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashOuts_Bets_BetId",
                        column: x => x.BetId,
                        principalTable: "Bets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameRounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GameSessionId = table.Column<int>(type: "integer", nullable: false),
                    Multiplier = table.Column<double>(type: "double precision", nullable: false),
                    IsCrashed = table.Column<bool>(type: "boolean", nullable: false),
                    Started = table.Column<bool>(type: "boolean", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRounds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrentRoundId = table.Column<int>(type: "integer", nullable: true),
                    ProviderId = table.Column<string>(type: "text", nullable: true),
                    BettingTime = table.Column<bool>(type: "boolean", nullable: false),
                    Started = table.Column<bool>(type: "boolean", nullable: false),
                    StartedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameSessions_GameRounds_CurrentRoundId",
                        column: x => x.CurrentRoundId,
                        principalTable: "GameRounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_GameRoundId",
                table: "Bets",
                column: "GameRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_CashOuts_BetId",
                table: "CashOuts",
                column: "BetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameRounds_GameSessionId",
                table: "GameRounds",
                column: "GameSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_CurrentRoundId",
                table: "GameSessions",
                column: "CurrentRoundId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_GameRounds_GameRoundId",
                table: "Bets",
                column: "GameRoundId",
                principalTable: "GameRounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameRounds_GameSessions_GameSessionId",
                table: "GameRounds",
                column: "GameSessionId",
                principalTable: "GameSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_GameRounds_CurrentRoundId",
                table: "GameSessions");

            migrationBuilder.DropTable(
                name: "CashOuts");

            migrationBuilder.DropTable(
                name: "Bets");

            migrationBuilder.DropTable(
                name: "GameRounds");

            migrationBuilder.DropTable(
                name: "GameSessions");
        }
    }
}
