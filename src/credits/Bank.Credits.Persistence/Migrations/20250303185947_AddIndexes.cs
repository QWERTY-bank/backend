using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Credits.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_IssuingCreditsPlans_Id",
                schema: "bank_plans",
                table: "IssuingCreditsPlans",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Credits_PlanId",
                schema: "bank_credits",
                table: "Credits",
                column: "PlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IssuingCreditsPlans_Id",
                schema: "bank_plans",
                table: "IssuingCreditsPlans");

            migrationBuilder.DropIndex(
                name: "IX_Credits_PlanId",
                schema: "bank_credits",
                table: "Credits");
        }
    }
}
