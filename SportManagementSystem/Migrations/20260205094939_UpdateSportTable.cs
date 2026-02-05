using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingSessions_Staffs_TrainerId",
                table: "TrainingSessions");

            migrationBuilder.AlterColumn<long>(
                name: "TrainerId",
                table: "TrainingSessions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingSessions_Staffs_TrainerId",
                table: "TrainingSessions",
                column: "TrainerId",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingSessions_Staffs_TrainerId",
                table: "TrainingSessions");

            migrationBuilder.AlterColumn<long>(
                name: "TrainerId",
                table: "TrainingSessions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingSessions_Staffs_TrainerId",
                table: "TrainingSessions",
                column: "TrainerId",
                principalTable: "Staffs",
                principalColumn: "Id");
        }
    }
}
