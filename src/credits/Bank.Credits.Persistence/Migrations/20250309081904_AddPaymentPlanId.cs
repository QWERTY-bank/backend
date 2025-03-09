using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bank.Credits.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentPlanId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PlanId",
                schema: "bank_credits",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PlanId",
                schema: "bank_credits",
                table: "Payments",
                column: "PlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_PlanId",
                schema: "bank_credits",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PlanId",
                schema: "bank_credits",
                table: "Payments");
        }
    }
}
