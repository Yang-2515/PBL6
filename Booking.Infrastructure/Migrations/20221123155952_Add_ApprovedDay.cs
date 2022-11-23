using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Infrastructure.Migrations
{
    public partial class Add_ApprovedDay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedOn",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedOn",
                table: "Bookings");
        }
    }
}
