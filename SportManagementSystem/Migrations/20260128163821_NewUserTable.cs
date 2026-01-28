using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class NewUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "UserAccounts",
                newName: "Email");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccounts_UserName",
                table: "UserAccounts",
                newName: "IX_UserAccounts_Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "UserAccounts",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccounts_Email",
                table: "UserAccounts",
                newName: "IX_UserAccounts_UserName");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Clients",
                type: "text",
                nullable: true);
        }
    }
}
