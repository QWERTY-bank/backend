using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Credits.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "PaymentDate",
                schema: "bank_credits",
                table: "Payments",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                schema: "bank_credits",
                table: "Payments");
        }
    }
}
