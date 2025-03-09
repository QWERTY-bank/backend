using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Credits.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLastInterestChargeDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "LastInterestChargeDate",
                schema: "bank_credits",
                table: "Credits",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastInterestChargeDate",
                schema: "bank_credits",
                table: "Credits");
        }
    }
}
