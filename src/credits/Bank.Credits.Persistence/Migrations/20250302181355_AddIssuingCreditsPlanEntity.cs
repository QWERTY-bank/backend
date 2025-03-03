using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bank.Credits.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIssuingCreditsPlanEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bank_plans");

            migrationBuilder.RenameColumn(
                name: "LoanAmount",
                schema: "bank_credits",
                table: "Credits",
                newName: "DebtAmount");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                schema: "bank_credits",
                table: "Credits",
                newName: "TakingDate");

            migrationBuilder.AddColumn<long>(
                name: "PlanId",
                schema: "bank_credits",
                table: "Credits",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "bank_credits",
                table: "Credits",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "IssuingCreditsPlans",
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
                    table.PrimaryKey("PK_IssuingCreditsPlans", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssuingCreditsPlans",
                schema: "bank_plans");

            migrationBuilder.DropColumn(
                name: "PlanId",
                schema: "bank_credits",
                table: "Credits");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "bank_credits",
                table: "Credits");

            migrationBuilder.RenameColumn(
                name: "TakingDate",
                schema: "bank_credits",
                table: "Credits",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "DebtAmount",
                schema: "bank_credits",
                table: "Credits",
                newName: "LoanAmount");
        }
    }
}
