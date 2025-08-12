using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pw.Clanner.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddUserLockoutFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LockoutReason",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutUntil",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockoutReason",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutUntil",
                table: "Users");
        }
    }
}
