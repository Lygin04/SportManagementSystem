using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportManagementSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBranchAndStaffTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Branches_BranchId",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_BranchId",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Staffs");

            migrationBuilder.AddColumn<long>(
                name: "AdminId",
                table: "Branches",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "BranchAdmins",
                columns: table => new
                {
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    StaffId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchAdmins", x => new { x.BranchId, x.StaffId });
                    table.ForeignKey(
                        name: "FK_BranchAdmins_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchAdmins_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchStaffs",
                columns: table => new
                {
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    StaffId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchStaffs", x => new { x.BranchId, x.StaffId });
                    table.ForeignKey(
                        name: "FK_BranchStaffs_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchStaffs_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_AdminId",
                table: "Branches",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchAdmins_StaffId",
                table: "BranchAdmins",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchStaffs_StaffId",
                table: "BranchStaffs",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Staffs_AdminId",
                table: "Branches",
                column: "AdminId",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Staffs_AdminId",
                table: "Branches");

            migrationBuilder.DropTable(
                name: "BranchAdmins");

            migrationBuilder.DropTable(
                name: "BranchStaffs");

            migrationBuilder.DropIndex(
                name: "IX_Branches_AdminId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Branches");

            migrationBuilder.AddColumn<long>(
                name: "BranchId",
                table: "Staffs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_BranchId",
                table: "Staffs",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Branches_BranchId",
                table: "Staffs",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id");
        }
    }
}
