using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedTrip.Infrastructure.Migrations
{
    public partial class AddedIsDeletedToComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bcb4f072-ecca-43c9-ab26-c060c6f364e4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8ae2a641-9d9f-49d3-a4ba-df1fa712f27b", "AQAAAAEAACcQAAAAEPQOV/MQYiQto5MYLlasnJ+h1RyCd95oFjq6kE0s208KlGZIofNpw0yzmPJgP0IFaw==", "6621745d-64de-4389-98ac-35f207c27f4d" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Comments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bcb4f072-ecca-43c9-ab26-c060c6f364e4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e605adff-7713-44ad-b5cb-8f9d2e26f28b", "AQAAAAEAACcQAAAAEOInfx6aFO7dCd8ISNJ4TNurihRq9Csl9DAXuCIrHl5yS1Bh5PXdJyTeAM99sIMTvw==", "e45ed72e-f25f-42b2-99d4-97e5dec7674a" });
        }
    }
}
