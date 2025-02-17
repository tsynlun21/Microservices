﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Purchases.DataAccesss.DataContext;

#nullable disable

namespace Purchases.DataAccesss.Migrations
{
    [DbContext(typeof(PurchasesDbContext))]
    partial class PurchasesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Purchases.DataAccesss.ContextEntities.CustomerEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Purchases.DataAccesss.ContextEntities.ExtraItemEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<long>("TransactionKey")
                        .HasColumnType("bigint");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("VehicleModel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TransactionKey");

                    b.ToTable("ExtraItemEntity");
                });

            modelBuilder.Entity("Purchases.DataAccesss.ContextEntities.ReceiptEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<long>("ShowroomId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric");

                    b.Property<long>("TransactionEntityKey")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TransactionEntityKey")
                        .IsUnique();

                    b.ToTable("ReceiptEntity");
                });

            modelBuilder.Entity("Purchases.DataAccesss.ContextEntities.TransactionEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("CustomerEntityKey")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsPerfomedByShowroom")
                        .HasColumnType("boolean");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CustomerEntityKey");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Purchases.DataAccesss.ContextEntities.VehicleEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Color")
                        .HasColumnType("integer");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<DateOnly>("ReleaseDate")
                        .HasColumnType("date");

                    b.Property<long>("TransactionKey")
                        .HasColumnType("bigint");

                    b.Property<string>("Vin")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TransactionKey")
                        .IsUnique();

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("Purchases.DataAccesss.ContextEntities.ExtraItemEntity", b =>
                {
                    b.HasOne("Purchases.DataAccesss.ContextEntities.TransactionEntity", "Transaction")
                        .WithMany("ExtraItems")
                        .HasForeignKey("TransactionKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("Purchases.DataAccesss.ContextEntities.ReceiptEntity", b =>
                {
                    b.HasOne("Purchases.DataAccesss.ContextEntities.TransactionEntity", "Transaction")
                        .WithOne("Receipt")
                        .HasForeignKey("Purchases.DataAccesss.ContextEntities.ReceiptEntity", "TransactionEntityKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("Purchases.DataAccesss.ContextEntities.TransactionEntity", b =>
                {
                    b.HasOne("Purchases.DataAccesss.ContextEntities.CustomerEntity", "CustomerEntity")
                        .WithMany("Transactions")
                        .HasForeignKey("CustomerEntityKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CustomerEntity");
                });

            modelBuilder.Entity("Purchases.DataAccesss.ContextEntities.VehicleEntity", b =>
                {
                    b.HasOne("Purchases.DataAccesss.ContextEntities.TransactionEntity", "Transaction")
                        .WithOne("Vehicle")
                        .HasForeignKey("Purchases.DataAccesss.ContextEntities.VehicleEntity", "TransactionKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("Purchases.DataAccesss.ContextEntities.CustomerEntity", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Purchases.DataAccesss.ContextEntities.TransactionEntity", b =>
                {
                    b.Navigation("ExtraItems");

                    b.Navigation("Receipt")
                        .IsRequired();

                    b.Navigation("Vehicle")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
