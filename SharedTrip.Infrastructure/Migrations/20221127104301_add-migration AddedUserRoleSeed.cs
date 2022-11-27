using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedTrip.Infrastructure.Migrations
{
    public partial class addmigrationAddedUserRoleSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0da87dbc-5868-4b99-927d-1366f8c0fdbf", "77db1f29-2c45-4a2c-a102-571ceac0a0ab", "User", "USER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bcb4f072-ecca-43c9-ab26-c060c6f364e4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e605adff-7713-44ad-b5cb-8f9d2e26f28b", "AQAAAAEAACcQAAAAEOInfx6aFO7dCd8ISNJ4TNurihRq9Csl9DAXuCIrHl5yS1Bh5PXdJyTeAM99sIMTvw==", "e45ed72e-f25f-42b2-99d4-97e5dec7674a" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0da87dbc-5868-4b99-927d-1366f8c0fdbf");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bcb4f072-ecca-43c9-ab26-c060c6f364e4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f1cb6dc9-beb9-4140-983c-4b169838382d", "AQAAAAEAACcQAAAAEDtUEfincIDsc79zq+eUz5GjesfUAtI9fxyNWr5gI1RdTzDrdj9ggftd98xuDRNB9A==", "a3c1f5e8-f639-4cff-8c6b-16b9353a6428" });
        }
    }
}
