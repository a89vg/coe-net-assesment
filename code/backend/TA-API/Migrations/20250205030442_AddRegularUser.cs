using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TA_API.Migrations
{
    /// <inheritdoc />
    public partial class AddRegularUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "FullName", "PasswordHash", "Username" },
                values: new object[] { 2, new DateTime(1989, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "alejandro.vazquez@unosquare.com", "Alejandro Vázquez Gógnora", "AQAAAAIAAYagAAAAEEOCuWEdhDbhYbrCSiQmdXZG9ZZHq1bcDo4Ku5klEDg8TkTpma+DciN19NO8dzlA6w==", "avazquez" });


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
