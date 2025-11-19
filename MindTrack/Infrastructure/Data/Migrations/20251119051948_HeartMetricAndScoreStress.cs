using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindTrack.Migrations
{
    /// <inheritdoc />
    public partial class HeartMetricAndScoreStress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeartMetrics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    HeartRate = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Rmssd = table.Column<double>(type: "BINARY_DOUBLE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeartMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeartMetrics_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StressScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Score = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Level = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SourceMetricId = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StressScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StressScores_HeartMetrics_SourceMetricId",
                        column: x => x.SourceMetricId,
                        principalTable: "HeartMetrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StressScores_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeartMetrics_UserId",
                table: "HeartMetrics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StressScores_SourceMetricId",
                table: "StressScores",
                column: "SourceMetricId");

            migrationBuilder.CreateIndex(
                name: "IX_StressScores_UserId",
                table: "StressScores",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StressScores");

            migrationBuilder.DropTable(
                name: "HeartMetrics");
        }
    }
}
