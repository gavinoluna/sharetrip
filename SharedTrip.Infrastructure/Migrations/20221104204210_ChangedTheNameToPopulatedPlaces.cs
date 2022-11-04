using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedTrip.Infrastructure.Migrations
{
    public partial class ChangedTheNameToPopulatedPlaces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Cities_EndDestinationId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Cities_StartDestinationId",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "PopulatedPlaces");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PopulatedPlaces",
                table: "PopulatedPlaces",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_PopulatedPlaces_EndDestinationId",
                table: "Trips",
                column: "EndDestinationId",
                principalTable: "PopulatedPlaces",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_PopulatedPlaces_StartDestinationId",
                table: "Trips",
                column: "StartDestinationId",
                principalTable: "PopulatedPlaces",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_PopulatedPlaces_EndDestinationId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_PopulatedPlaces_StartDestinationId",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PopulatedPlaces",
                table: "PopulatedPlaces");

            migrationBuilder.RenameTable(
                name: "PopulatedPlaces",
                newName: "Cities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Cities_EndDestinationId",
                table: "Trips",
                column: "EndDestinationId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Cities_StartDestinationId",
                table: "Trips",
                column: "StartDestinationId",
                principalTable: "Cities",
                principalColumn: "Id");
        }
    }
}
