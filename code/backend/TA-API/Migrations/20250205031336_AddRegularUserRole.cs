using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TA_API.Migrations
{
    /// <inheritdoc />
    public partial class AddRegularUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Role",
                value: "Admin");

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "Role", "Username" },
                values: new object[] { 2, "User", "avazquez" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Role",
                value: "admin");
        }
    }
}
