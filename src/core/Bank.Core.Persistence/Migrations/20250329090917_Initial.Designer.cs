﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Bank.Core.Domain.Transactions;
using Bank.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bank.Core.Persistence.Migrations
{
    [DbContext(typeof(CoreDbContext))]
    [Migration("20250329090917_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("bank_core")
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Bank.Core.Domain.Accounts.AccountBaseEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean")
                        .HasColumnName("is_closed");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<int>("type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_account_entity");

                    b.ToTable("account_entity", "bank_core");

                    b.HasDiscriminator<int>("type");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Bank.Core.Domain.Accounts.AccountCurrencyEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AccountId")
                        .HasColumnType("bigint")
                        .HasColumnName("account_id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_account_currency_entity");

                    b.HasIndex("AccountId")
                        .HasDatabaseName("ix_account_currency_entity_account_id");

                    b.HasIndex("Code")
                        .HasDatabaseName("ix_account_currency_entity_code");

                    b.ToTable("account_currency_entity", "bank_core");
                });

            modelBuilder.Entity("Bank.Core.Domain.Currencies.CurrencyEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.HasKey("Id")
                        .HasName("pk_currency_entity");

                    b.HasAlternateKey("Code")
                        .HasName("ak_currency_entity_code");

                    b.ToTable("currency_entity", "bank_core");
                });

            modelBuilder.Entity("Bank.Core.Domain.Transactions.TransactionEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("AccountId")
                        .HasColumnType("bigint")
                        .HasColumnName("account_id");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date")
                        .HasDefaultValueSql("now()");

                    b.Property<IReadOnlyCollection<TransactionCurrency>>("Currencies")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("currencies");

                    b.Property<bool>("IsCancel")
                        .HasColumnType("boolean")
                        .HasColumnName("is_cancel");

                    b.Property<Guid>("Key")
                        .HasColumnType("uuid")
                        .HasColumnName("key");

                    b.Property<DateTime>("OperationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("operation_date");

                    b.Property<Guid?>("ParentKey")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_key");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_transaction_entity");

                    b.HasAlternateKey("Key")
                        .HasName("ak_transaction_entity_key");

                    b.HasIndex("ParentKey")
                        .HasDatabaseName("ix_transaction_entity_parent_key");

                    b.HasIndex("AccountId", "OperationDate")
                        .HasDatabaseName("ix_transaction_entity_account_id_operation_date");

                    b.ToTable("transaction_entity", "bank_core");

                    b.HasDiscriminator<int>("Type");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Bank.Core.Domain.Accounts.PersonalAccountEntity", b =>
                {
                    b.HasBaseType("Bank.Core.Domain.Accounts.AccountBaseEntity");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasIndex("UserId", "Title")
                        .IsUnique()
                        .HasDatabaseName("ix_account_entity_user_id_title");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("Bank.Core.Domain.Accounts.UnitAccountEntity", b =>
                {
                    b.HasBaseType("Bank.Core.Domain.Accounts.AccountBaseEntity");

                    b.Property<Guid>("UnitId")
                        .HasColumnType("uuid")
                        .HasColumnName("unit_id");

                    b.HasIndex("UnitId")
                        .HasDatabaseName("ix_account_entity_unit_id");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Bank.Core.Domain.Transactions.DepositTransactionEntity", b =>
                {
                    b.HasBaseType("Bank.Core.Domain.Transactions.TransactionEntity");

                    b.ToTable("transaction_entity", "bank_core");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("Bank.Core.Domain.Transactions.WithdrawTransactionEntity", b =>
                {
                    b.HasBaseType("Bank.Core.Domain.Transactions.TransactionEntity");

                    b.ToTable("transaction_entity", "bank_core");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Bank.Core.Domain.Accounts.AccountCurrencyEntity", b =>
                {
                    b.HasOne("Bank.Core.Domain.Accounts.AccountBaseEntity", null)
                        .WithMany("AccountCurrencies")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_account_currency_entity_account_entity_account_id");

                    b.HasOne("Bank.Core.Domain.Currencies.CurrencyEntity", null)
                        .WithMany()
                        .HasForeignKey("Code")
                        .HasPrincipalKey("Code")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_account_currency_entity_currency_entity_code");
                });

            modelBuilder.Entity("Bank.Core.Domain.Transactions.TransactionEntity", b =>
                {
                    b.HasOne("Bank.Core.Domain.Accounts.AccountBaseEntity", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_transaction_entity_account_entity_account_id");

                    b.HasOne("Bank.Core.Domain.Transactions.TransactionEntity", null)
                        .WithMany("ChildTransactions")
                        .HasForeignKey("ParentKey")
                        .HasPrincipalKey("Key")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("fk_transaction_entity_transaction_entity_parent_key");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Bank.Core.Domain.Accounts.AccountBaseEntity", b =>
                {
                    b.Navigation("AccountCurrencies");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Bank.Core.Domain.Transactions.TransactionEntity", b =>
                {
                    b.Navigation("ChildTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
