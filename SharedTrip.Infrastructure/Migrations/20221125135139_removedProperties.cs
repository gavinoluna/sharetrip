using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedTrip.Infrastructure.Migrations
{
    public partial class removedProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountOfTripsAsDriver",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CountOfTripsAsPassenger",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountOfTripsAsDriver",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountOfTripsAsPassenger",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
