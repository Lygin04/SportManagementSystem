using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddClientTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Clients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Staffs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Clients",
                type: "uuid",
                nullable: true);
        }
    }
}
