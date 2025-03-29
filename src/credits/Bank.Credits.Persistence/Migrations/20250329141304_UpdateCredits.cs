using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Credits.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCredits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentsInfo_Payment",
                schema: "bank_credits",
                table: "Credits");

            migrationBuilder.RenameColumn(
                name: "TakingDate",
                schema: "bank_credits",
                table: "Credits",
                newName: "PaymentsInfo_TakingDate");

            migrationBuilder.RenameColumn(
                name: "PeriodDays",
                schema: "bank_credits",
                table: "Credits",
                newName: "PaymentsInfo_PeriodDays");

            migrationBuilder.RenameColumn(
                name: "LastInterestChargeDate",
                schema: "bank_credits",
                table: "Credits",
                newName: "PaymentsInfo_LastInterestChargeDate");

            migrationBuilder.RenameColumn(
                name: "DebtAmount",
                schema: "bank_credits",
                table: "Credits",
                newName: "PaymentsInfo_DebtAmount");

            migrationBuilder.RenameColumn(
                name: "PaymentsInfo_LastPayment",
                schema: "bank_credits",
                table: "Credits",
                newName: "PaymentsInfo_EqualPayment");

            migrationBuilder.AddColumn<string>(
                name: "PaymentsInfo_DebtsWithInterest",
                schema: "bank_credits",
                table: "Credits",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "PaymentsInfo_NextPaymentDate",
                schema: "bank_credits",
                table: "Credits",
                type: "date",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "bank_credits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Rating = table.Column<double>(type: "double precision", nullable: false),
                    RatingIsActual = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credits_UserId",
                schema: "bank_credits",
                table: "Credits",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Credits_Users_UserId",
                schema: "bank_credits",
                table: "Credits",
                column: "UserId",
                principalSchema: "bank_credits",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credits_Users_UserId",
                schema: "bank_credits",
                table: "Credits");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "bank_credits");

            migrationBuilder.DropIndex(
                name: "IX_Credits_UserId",
                schema: "bank_credits",
                table: "Credits");

            migrationBuilder.DropColumn(
                name: "PaymentsInfo_DebtsWithInterest",
                schema: "bank_credits",
                table: "Credits");

            migrationBuilder.DropColumn(
                name: "PaymentsInfo_NextPaymentDate",
                schema: "bank_credits",
                table: "Credits");

            migrationBuilder.RenameColumn(
                name: "PaymentsInfo_TakingDate",
                schema: "bank_credits",
                table: "Credits",
                newName: "TakingDate");

            migrationBuilder.RenameColumn(
                name: "PaymentsInfo_PeriodDays",
                schema: "bank_credits",
                table: "Credits",
                newName: "PeriodDays");

            migrationBuilder.RenameColumn(
                name: "PaymentsInfo_LastInterestChargeDate",
                schema: "bank_credits",
                table: "Credits",
                newName: "LastInterestChargeDate");

            migrationBuilder.RenameColumn(
                name: "PaymentsInfo_DebtAmount",
                schema: "bank_credits",
                table: "Credits",
                newName: "DebtAmount");

            migrationBuilder.RenameColumn(
                name: "PaymentsInfo_EqualPayment",
                schema: "bank_credits",
                table: "Credits",
                newName: "PaymentsInfo_LastPayment");

            migrationBuilder.AddColumn<int>(
                name: "PaymentsInfo_Payment",
                schema: "bank_credits",
                table: "Credits",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
