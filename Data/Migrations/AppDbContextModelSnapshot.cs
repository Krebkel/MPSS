﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("mpss")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Contracts.EmployeeEntities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal?>("AccountNumber")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal?>("BIK")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTimeOffset>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal?>("INN")
                        .HasColumnType("numeric(20,0)");

                    b.Property<bool>("IsDriver")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<decimal?>("Passport")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.ToTable("Employees", "mpss");
                });

            modelBuilder.Entity("Contracts.EmployeeEntities.EmployeeShift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset?>("Arrival")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("ConsiderTravel")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("Departure")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<float?>("HoursWorked")
                        .HasColumnType("float");

                    b.Property<int?>("ISN")
                        .HasColumnType("integer");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.Property<float?>("TravelTime")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ProjectId");

                    b.ToTable("EmployeeShifts", "mpss");
                });

            modelBuilder.Entity("Contracts.ProductEntities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Products", "mpss");
                });

            modelBuilder.Entity("Contracts.ProductEntities.ProductComponent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int?>("Quantity")
                        .HasColumnType("integer");

                    b.Property<float?>("Weight")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductComponents", "mpss");
                });

            modelBuilder.Entity("Contracts.ProductEntities.ProjectProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("Markup")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectProducts", "mpss");
                });

            modelBuilder.Entity("Contracts.ProjectEntities.Counteragent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal?>("AccountNumber")
                        .HasColumnType("numeric(20,0)");

                    b.Property<long?>("BIK")
                        .HasColumnType("bigint");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<long?>("INN")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<long?>("OGRN")
                        .HasColumnType("bigint");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.HasKey("Id");

                    b.ToTable("Counteragents", "mpss");
                });

            modelBuilder.Entity("Contracts.ProjectEntities.Expense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double?>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<bool>("IsPaidByCompany")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Expenses", "mpss");
                });

            modelBuilder.Entity("Contracts.ProjectEntities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("CounteragentId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("DeadlineDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<float>("ManagerShare")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProjectStatus")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ResponsibleEmployeeId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CounteragentId");

                    b.HasIndex("ResponsibleEmployeeId");

                    b.ToTable("Projects", "mpss");
                });

            modelBuilder.Entity("Contracts.ProjectEntities.ProjectSuspension", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("DateSuspended")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectSuspensions", "mpss");
                });

            modelBuilder.Entity("Contracts.EmployeeEntities.EmployeeShift", b =>
                {
                    b.HasOne("Contracts.EmployeeEntities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Contracts.ProjectEntities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Contracts.ProductEntities.ProductComponent", b =>
                {
                    b.HasOne("Contracts.ProductEntities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Contracts.ProductEntities.ProjectProduct", b =>
                {
                    b.HasOne("Contracts.ProductEntities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Contracts.ProjectEntities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Contracts.ProjectEntities.Expense", b =>
                {
                    b.HasOne("Contracts.ProjectEntities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Contracts.ProjectEntities.Project", b =>
                {
                    b.HasOne("Contracts.ProjectEntities.Counteragent", "Counteragent")
                        .WithMany()
                        .HasForeignKey("CounteragentId");

                    b.HasOne("Contracts.EmployeeEntities.Employee", "ResponsibleEmployee")
                        .WithMany()
                        .HasForeignKey("ResponsibleEmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Counteragent");

                    b.Navigation("ResponsibleEmployee");
                });

            modelBuilder.Entity("Contracts.ProjectEntities.ProjectSuspension", b =>
                {
                    b.HasOne("Contracts.ProjectEntities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });
#pragma warning restore 612, 618
        }
    }
}
