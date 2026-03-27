using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportManagementSystem.Persistence.Migrations
{
    public partial class AddMembershipTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MembershipTemplateId",
                table: "Memberships",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MembershipTemplates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    SportServiceId = table.Column<long>(type: "bigint", nullable: false),
                    ServicePriceId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DurationDays = table.Column<int>(type: "integer", nullable: false),
                    VisitLimit = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembershipTemplates_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MembershipTemplates_SportServices_SportServiceId",
                        column: x => x.SportServiceId,
                        principalTable: "SportServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MembershipTemplates_ServicePrices_ServicePriceId",
                        column: x => x.ServicePriceId,
                        principalTable: "ServicePrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_MembershipTemplateId",
                table: "Memberships",
                column: "MembershipTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipTemplates_BranchId",
                table: "MembershipTemplates",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipTemplates_SportServiceId",
                table: "MembershipTemplates",
                column: "SportServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipTemplates_ServicePriceId",
                table: "MembershipTemplates",
                column: "ServicePriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_MembershipTemplates_MembershipTemplateId",
                table: "Memberships",
                column: "MembershipTemplateId",
                principalTable: "MembershipTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_MembershipTemplates_MembershipTemplateId",
                table: "Memberships");

            migrationBuilder.DropTable(
                name: "MembershipTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_MembershipTemplateId",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "MembershipTemplateId",
                table: "Memberships");
        }
    }
}
