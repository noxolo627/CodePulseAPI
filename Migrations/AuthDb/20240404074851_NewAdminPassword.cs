using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodePulse.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class NewAdminPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "799ac999-ddf2-47ed-af24-19c8921cde17",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2612ed9a-146f-4e75-b6a5-3ad7d725f486", "AQAAAAIAAYagAAAAEAEduw97p1vOVE0YQKlXAVEeHEp2xUbXWpM4Y4oC3sWFcS5YkfQ95mtWMr5py3tbsQ==", "d4d626b0-412f-4682-9f50-8549aedb91b9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "799ac999-ddf2-47ed-af24-19c8921cde17",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ff796d84-5c2e-4693-9f13-9af82c65f222", "AQAAAAIAAYagAAAAEO6OiE3SWOqYjLSdgzoT8RLV9BwlDBXERXfui3xP7Y4Bvr/yWvK/BxE9+RkV6GI8bw==", "26eb0493-f659-4783-a894-bc04bbc4d790" });
        }
    }
}
