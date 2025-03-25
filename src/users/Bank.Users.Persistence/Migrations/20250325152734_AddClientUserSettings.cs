using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddClientUserSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientUserSettings",
                schema: "bank_users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsNightTheme = table.Column<bool>(type: "boolean", nullable: false),
                    HiddenAccounts = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientUserSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientUserSettings_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "bank_users",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientUserSettings_UserId",
                schema: "bank_users",
                table: "ClientUserSettings",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientUserSettings",
                schema: "bank_users");
        }
    }
}
