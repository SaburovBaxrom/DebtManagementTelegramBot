using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBotApartment.Migrations
{
    public partial class card : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "UserToken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "UserToken");
        }
    }
}
