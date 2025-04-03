using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Credits.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCredits2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                schema: "bank_credits",
                table: "Credits");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyCode",
                schema: "bank_credits",
                table: "Credits",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                schema: "bank_credits",
                table: "Credits");

            migrationBuilder.AddColumn<Guid>(
                name: "Key",
                schema: "bank_credits",
                table: "Credits",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
