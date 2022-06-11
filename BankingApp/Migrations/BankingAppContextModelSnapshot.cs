﻿// <auto-generated />
using System;
using BankingApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankingApp.Migrations
{
    [DbContext(typeof(BankingAppContext))]
    partial class BankingAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BankingApp.Models.Account", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("Cents")
                        .HasColumnType("int");

                    b.Property<int>("Dollars")
                        .HasColumnType("int");

                    b.Property<int>("LoginID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("LoginID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("BankingApp.Models.Login", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BankingApp.Models.Transfers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Cents")
                        .HasColumnType("int");

                    b.Property<int>("DepositID")
                        .HasColumnType("int");

                    b.Property<int>("Deposits")
                        .HasColumnType("int");

                    b.Property<int>("Dollars")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WithdrawAccountID")
                        .HasColumnType("int");

                    b.Property<int>("WithdrawID")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.HasKey("Id");

                    b.HasIndex("Deposits");

                    b.HasIndex("WithdrawAccountID");

                    b.ToTable("Transfers");
                });

            modelBuilder.Entity("BankingApp.Models.Account", b =>
                {
                    b.HasOne("BankingApp.Models.Login", "Login")
                        .WithMany("Accounts")
                        .HasForeignKey("LoginID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Login");
                });

            modelBuilder.Entity("BankingApp.Models.Transfers", b =>
                {
                    b.HasOne("BankingApp.Models.Account", "DepositAccount")
                        .WithMany("Deposits")
                        .HasForeignKey("Deposits")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BankingApp.Models.Account", "WithdrawAccount")
                        .WithMany("Withdrawals")
                        .HasForeignKey("WithdrawAccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DepositAccount");

                    b.Navigation("WithdrawAccount");
                });

            modelBuilder.Entity("BankingApp.Models.Account", b =>
                {
                    b.Navigation("Deposits");

                    b.Navigation("Withdrawals");
                });

            modelBuilder.Entity("BankingApp.Models.Login", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
