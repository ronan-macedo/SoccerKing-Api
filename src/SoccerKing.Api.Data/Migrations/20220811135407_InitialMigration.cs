using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoccerKing.Api.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreateAt", "Email", "Name", "Password", "UpdateAt" },
                values: new object[] { new Guid("1e998955-798b-4008-8d61-0db0cd92022b"), new DateTime(2022, 8, 11, 13, 54, 7, 549, DateTimeKind.Utc).AddTicks(4383), "admin@email.com", "Admin", "$2a$11$PuLEtW60Cq6ga5va5Lb0IuvEJYQ.Od.ThZsYFRosKDLUwzdp.jski", null });

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
