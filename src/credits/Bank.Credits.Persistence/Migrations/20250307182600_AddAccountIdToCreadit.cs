using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Credits.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountIdToCreadit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                schema: "bank_credits",
                table: "Credits",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "bank_credits",
                table: "Credits");
        }
    }
}
