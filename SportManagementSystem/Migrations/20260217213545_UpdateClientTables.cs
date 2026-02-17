using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClientTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Branches_BranchId1",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_BranchId1",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "BranchId1",
                table: "Staffs");

            migrationBuilder.AlterColumn<long>(
                name: "BranchId",
                table: "Staffs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_BranchId",
                table: "Staffs",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Branches_BranchId",
                table: "Staffs",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Branches_BranchId",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_BranchId",
                table: "Staffs");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Staffs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "BranchId1",
                table: "Staffs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_BranchId1",
                table: "Staffs",
                column: "BranchId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Branches_BranchId1",
                table: "Staffs",
                column: "BranchId1",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
