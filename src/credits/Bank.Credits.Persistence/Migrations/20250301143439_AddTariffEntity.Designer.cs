﻿// <auto-generated />
using System;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bank.Credits.Persistence.Migrations
{
    [DbContext(typeof(CreditsDbContext))]
    [Migration("20250301143439_AddTariffEntity")]
    partial class AddTariffEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("bank_credits")
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Bank.Credits.Domain.Tariffs.Tariff", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DeleteDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("InterestRate")
                        .HasColumnType("numeric");

                    b.Property<int>("InterestRateType")
                        .HasColumnType("integer");

                    b.Property<int>("MaxPeriodDays")
                        .HasColumnType("integer");

                    b.Property<int>("MinPeriodDays")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tariffs", "bank_credits");
                });
#pragma warning restore 612, 618
        }
    }
}
