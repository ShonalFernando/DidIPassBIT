using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BITChecker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GPAs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateCalculated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GPACalculated = table.Column<decimal>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GPAs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Semester = table.Column<int>(type: "INTEGER", nullable: false),
                    Credit = table.Column<int>(type: "INTEGER", nullable: false),
                    GPACredit = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseType = table.Column<char>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerSubjectScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    GPAId = table.Column<int>(type: "INTEGER", nullable: false),
                    PointValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    QaulityPoint = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerSubjectScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerSubjectScores_GPAs_GPAId",
                        column: x => x.GPAId,
                        principalTable: "GPAs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerSubjectScores_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PerSubjectScores_GPAId",
                table: "PerSubjectScores",
                column: "GPAId");

            migrationBuilder.CreateIndex(
                name: "IX_PerSubjectScores_SubjectId",
                table: "PerSubjectScores",
                column: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PerSubjectScores");

            migrationBuilder.DropTable(
                name: "GPAs");

            migrationBuilder.DropTable(
                name: "Subjects");
        }
    }
}
