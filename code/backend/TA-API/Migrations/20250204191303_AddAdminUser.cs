using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TA_API.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserRoles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "Id");

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "Role", "Username" },
                values: new object[] { 1, "admin", "admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "FullName", "PasswordHash", "Username" },
                values: new object[] { 1, new DateTime(1989, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@unosquare.com", "Alejandro Vázquez Góngora", "AQAAAAIAAYagAAAAEPqCWJ7ya9WKGosM3zc8YI8gLojDSX+psFE9t73BstHwFT5XwOFFlTQwC65YvoK0sQ==", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyColumnType: "INTEGER",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserRoles");
        }
    }
}
