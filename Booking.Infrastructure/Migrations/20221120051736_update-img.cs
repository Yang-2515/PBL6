using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Infrastructure.Migrations
{
    public partial class updateimg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Reviews");

            migrationBuilder.AddColumn<string>(
                name: "ImgId",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgId",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgId",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ImgId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ImgId",
                table: "Locations");

            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
