using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedTrip.Infrastructure.Migrations
{
    public partial class AddedCountOfSeatsToTrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountOfSeats",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountOfSeats",
                table: "Trips");
        }
    }
}
