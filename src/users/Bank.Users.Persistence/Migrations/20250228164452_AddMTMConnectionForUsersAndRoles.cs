using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMTMConnectionForUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_UserEntityId",
                schema: "bank_users",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_UserEntityId",
                schema: "bank_users",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                schema: "bank_users",
                table: "Roles");

            migrationBuilder.CreateTable(
                name: "RoleUserEntity",
                schema: "bank_users",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUserEntity", x => new { x.RolesId, x.UserEntityId });
                    table.ForeignKey(
                        name: "FK_RoleUserEntity_Roles_RolesId",
                        column: x => x.RolesId,
                        principalSchema: "bank_users",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUserEntity_Users_UserEntityId",
                        column: x => x.UserEntityId,
                        principalSchema: "bank_users",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleUserEntity_UserEntityId",
                schema: "bank_users",
                table: "RoleUserEntity",
                column: "UserEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleUserEntity",
                schema: "bank_users");

            migrationBuilder.AddColumn<Guid>(
                name: "UserEntityId",
                schema: "bank_users",
                table: "Roles",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserEntityId",
                schema: "bank_users",
                table: "Roles",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_UserEntityId",
                schema: "bank_users",
                table: "Roles",
                column: "UserEntityId",
                principalSchema: "bank_users",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
