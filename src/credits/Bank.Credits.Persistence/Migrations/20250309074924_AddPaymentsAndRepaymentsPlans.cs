using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bank.Credits.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentsAndRepaymentsPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentsPlans",
                schema: "bank_plans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromPlanId = table.Column<long>(type: "bigint", nullable: false),
                    ToPlanId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    StatusUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RepaymentPlans",
                schema: "bank_plans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromPlanId = table.Column<long>(type: "bigint", nullable: false),
                    ToPlanId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    StatusUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepaymentPlans", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsPlans_Id",
                schema: "bank_plans",
                table: "PaymentsPlans",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RepaymentPlans_Id",
                schema: "bank_plans",
                table: "RepaymentPlans",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentsPlans",
                schema: "bank_plans");

            migrationBuilder.DropTable(
                name: "RepaymentPlans",
                schema: "bank_plans");
        }
    }
}
