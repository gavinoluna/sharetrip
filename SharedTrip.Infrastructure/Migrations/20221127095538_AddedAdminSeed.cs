using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedTrip.Infrastructure.Migrations
{
    public partial class AddedAdminSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePictureUrl", "Rating", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "bcb4f072-ecca-43c9-ab26-c060c6f364e4", 0, "e5a02280-12c1-4f93-ba15-715020786128", "admin@abv.bg", false, "Great", "Admin", false, null, "admin@abv.bg", "admin@abv.bg", "AQAAAAEAACcQAAAAEP1OuujYlYa4+ajEWpffHNF55HHkLpXEEtL79iMYX/oaY5xDyfPNbW+ixm9fnjTJEA==", "1234567899", false, "https://cdn-icons-png.flaticon.com/512/3048/3048127.png", 0.0, "c96c5587-663e-4507-95e8-af1074ddf652", false, "admin@abv.bg" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bcb4f072-ecca-43c9-ab26-c060c6f364e4");
        }
    }
}
