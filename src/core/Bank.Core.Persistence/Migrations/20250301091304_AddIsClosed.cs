using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Core.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsClosed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_account_entity_user_id",
                schema: "bank_core",
                table: "account_entity");

            migrationBuilder.AddColumn<bool>(
                name: "is_closed",
                schema: "bank_core",
                table: "account_entity",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_account_entity_user_id_title",
                schema: "bank_core",
                table: "account_entity",
                columns: new[] { "user_id", "title" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_account_entity_user_id_title",
                schema: "bank_core",
                table: "account_entity");

            migrationBuilder.DropColumn(
                name: "is_closed",
                schema: "bank_core",
                table: "account_entity");

            migrationBuilder.CreateIndex(
                name: "ix_account_entity_user_id",
                schema: "bank_core",
                table: "account_entity",
                column: "user_id",
                unique: true);
        }
    }
}
