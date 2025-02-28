using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBlockedToUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                schema: "bank_users",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlocked",
                schema: "bank_users",
                table: "Users");
        }
    }
}
