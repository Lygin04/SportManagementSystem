using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportManagementSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateMembershipTemplateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeZoneId",
                table: "TrainingSessions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TimeZoneId",
                table: "Bookings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeZoneId",
                table: "TrainingSessions");

            migrationBuilder.DropColumn(
                name: "TimeZoneId",
                table: "Bookings");
        }
    }
}
