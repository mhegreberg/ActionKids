using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActionKids.Migrations
{
    /// <inheritdoc />
    public partial class ServiceStop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ServiceStop",
                table: "Services",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceStop",
                table: "Services");
        }
    }
}
