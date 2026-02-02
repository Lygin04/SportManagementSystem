using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTabels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "TrainingSessions");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Staffs");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndedDate",
                table: "TrainingSessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedDate",
                table: "TrainingSessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Staffs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BranchId1",
                table: "Staffs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_BranchId1",
                table: "Staffs",
                column: "BranchId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Branches_BranchId1",
                table: "Staffs",
                column: "BranchId1",
                principalTable: "Branches",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Branches_BranchId1",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_BranchId1",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "EndedDate",
                table: "TrainingSessions");

            migrationBuilder.DropColumn(
                name: "StartedDate",
                table: "TrainingSessions");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "BranchId1",
                table: "Staffs");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "TrainingSessions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Staffs",
                type: "text",
                nullable: true);
        }
    }
}
