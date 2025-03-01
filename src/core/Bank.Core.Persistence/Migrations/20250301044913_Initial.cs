using System;
using System.Collections.Generic;
using Bank.Core.Domain.Transactions;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bank.Core.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bank_core");

            migrationBuilder.CreateTable(
                name: "account_entity",
                schema: "bank_core",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    unit_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account_entity", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "currency_entity",
                schema: "bank_core",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currency_entity", x => x.id);
                    table.UniqueConstraint("ak_currency_entity_code", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "transaction_entity",
                schema: "bank_core",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    parent_key = table.Column<Guid>(type: "uuid", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    operation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_cancel = table.Column<bool>(type: "boolean", nullable: false),
                    account_id = table.Column<long>(type: "bigint", nullable: false),
                    currencies = table.Column<List<TransactionCurrency>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transaction_entity", x => x.id);
                    table.UniqueConstraint("ak_transaction_entity_key", x => x.key);
                    table.ForeignKey(
                        name: "fk_transaction_entity_account_entity_account_id",
                        column: x => x.account_id,
                        principalSchema: "bank_core",
                        principalTable: "account_entity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_transaction_entity_transaction_entity_parent_key",
                        column: x => x.parent_key,
                        principalSchema: "bank_core",
                        principalTable: "transaction_entity",
                        principalColumn: "key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "account_currency_entity",
                schema: "bank_core",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    value = table.Column<decimal>(type: "numeric", nullable: false),
                    account_id = table.Column<long>(type: "bigint", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account_currency_entity", x => x.id);
                    table.ForeignKey(
                        name: "fk_account_currency_entity_account_entity_account_id",
                        column: x => x.account_id,
                        principalSchema: "bank_core",
                        principalTable: "account_entity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_account_currency_entity_currency_entity_code",
                        column: x => x.code,
                        principalSchema: "bank_core",
                        principalTable: "currency_entity",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_account_currency_entity_account_id",
                schema: "bank_core",
                table: "account_currency_entity",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "ix_account_currency_entity_code",
                schema: "bank_core",
                table: "account_currency_entity",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "ix_account_entity_unit_id",
                schema: "bank_core",
                table: "account_entity",
                column: "unit_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_account_entity_user_id",
                schema: "bank_core",
                table: "account_entity",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_transaction_entity_account_id_operation_date",
                schema: "bank_core",
                table: "transaction_entity",
                columns: new[] { "account_id", "operation_date" });

            migrationBuilder.CreateIndex(
                name: "ix_transaction_entity_parent_key",
                schema: "bank_core",
                table: "transaction_entity",
                column: "parent_key");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_currency_entity",
                schema: "bank_core");

            migrationBuilder.DropTable(
                name: "transaction_entity",
                schema: "bank_core");

            migrationBuilder.DropTable(
                name: "currency_entity",
                schema: "bank_core");

            migrationBuilder.DropTable(
                name: "account_entity",
                schema: "bank_core");
        }
    }
}
