using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBotApartment.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OwnerGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<long>(type: "bigint", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OwnerToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<long>(type: "bigint", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerToken", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OwnerGroup");

            migrationBuilder.DropTable(
                name: "OwnerToken");
        }
    }
}
