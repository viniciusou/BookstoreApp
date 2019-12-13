using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookstoreApp.API.Migrations
{
    public partial class AddedDateReleased : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateReleased",
                table: "Books",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateReleased",
                table: "Books");
        }
    }
}
