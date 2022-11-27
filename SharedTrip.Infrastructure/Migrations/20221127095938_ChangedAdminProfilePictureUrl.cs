using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedTrip.Infrastructure.Migrations
{
    public partial class ChangedAdminProfilePictureUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bcb4f072-ecca-43c9-ab26-c060c6f364e4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "ProfilePictureUrl", "SecurityStamp" },
                values: new object[] { "f1cb6dc9-beb9-4140-983c-4b169838382d", "AQAAAAEAACcQAAAAEDtUEfincIDsc79zq+eUz5GjesfUAtI9fxyNWr5gI1RdTzDrdj9ggftd98xuDRNB9A==", "https://res.cloudinary.com/dftfu5p0r/image/upload/v1669543091/3048127_t9hrkf.png", "a3c1f5e8-f639-4cff-8c6b-16b9353a6428" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bcb4f072-ecca-43c9-ab26-c060c6f364e4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "ProfilePictureUrl", "SecurityStamp" },
                values: new object[] { "e5a02280-12c1-4f93-ba15-715020786128", "AQAAAAEAACcQAAAAEP1OuujYlYa4+ajEWpffHNF55HHkLpXEEtL79iMYX/oaY5xDyfPNbW+ixm9fnjTJEA==", "https://cdn-icons-png.flaticon.com/512/3048/3048127.png", "c96c5587-663e-4507-95e8-af1074ddf652" });
        }
    }
}
