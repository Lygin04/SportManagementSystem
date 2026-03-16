using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportManagementSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BranchId",
                table: "SportServices",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SportServices_BranchId",
                table: "SportServices",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_SportServices_Branches_BranchId",
                table: "SportServices",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SportServices_Branches_BranchId",
                table: "SportServices");

            migrationBuilder.DropIndex(
                name: "IX_SportServices_BranchId",
                table: "SportServices");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "SportServices");
        }
    }
}
