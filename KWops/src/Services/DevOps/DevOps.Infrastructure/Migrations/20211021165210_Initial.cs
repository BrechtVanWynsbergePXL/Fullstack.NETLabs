using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DevOps.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Percentage",
                columns: table => new
                {
                    _value = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Percentage", x => x._value);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Developers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating_value = table.Column<double>(type: "float", nullable: true),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Developers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Developers_Percentage_Rating_value",
                        column: x => x.Rating_value,
                        principalTable: "Percentage",
                        principalColumn: "_value",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Developers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Developers_Rating_value",
                table: "Developers",
                column: "Rating_value");

            migrationBuilder.CreateIndex(
                name: "IX_Developers_TeamId",
                table: "Developers",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Developers");

            migrationBuilder.DropTable(
                name: "Percentage");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
