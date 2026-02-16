using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddImagesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Branches_BranchId1",
                table: "Staffs");

            migrationBuilder.AlterColumn<long>(
                name: "BranchId1",
                table: "Staffs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Staffs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AvatarId",
                table: "Staffs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Staffs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DbBranchId",
                table: "Images",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DbEquipmentId",
                table: "Images",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DbRoomId",
                table: "Images",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AvatarId",
                table: "Clients",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Clients",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_AvatarId",
                table: "Staffs",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_DbBranchId",
                table: "Images",
                column: "DbBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_DbEquipmentId",
                table: "Images",
                column: "DbEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_DbRoomId",
                table: "Images",
                column: "DbRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_AvatarId",
                table: "Clients",
                column: "AvatarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Images_AvatarId",
                table: "Clients",
                column: "AvatarId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Branches_DbBranchId",
                table: "Images",
                column: "DbBranchId",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Equipments_DbEquipmentId",
                table: "Images",
                column: "DbEquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Rooms_DbRoomId",
                table: "Images",
                column: "DbRoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Branches_BranchId1",
                table: "Staffs",
                column: "BranchId1",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Images_AvatarId",
                table: "Staffs",
                column: "AvatarId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Images_AvatarId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Branches_DbBranchId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Equipments_DbEquipmentId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Rooms_DbRoomId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Branches_BranchId1",
                table: "Staffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Images_AvatarId",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_AvatarId",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Images_DbBranchId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_DbEquipmentId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_DbRoomId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Clients_AvatarId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "DbBranchId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "DbEquipmentId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "DbRoomId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Clients");

            migrationBuilder.AlterColumn<long>(
                name: "BranchId1",
                table: "Staffs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Staffs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Branches_BranchId1",
                table: "Staffs",
                column: "BranchId1",
                principalTable: "Branches",
                principalColumn: "Id");
        }
    }
}
