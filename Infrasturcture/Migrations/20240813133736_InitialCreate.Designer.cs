﻿// <auto-generated />
using System;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(LeaveDbContext))]
    [Migration("20240813133736_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.DTOs.LeaveRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<int?>("EmployeeId1")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("EmployeeId1");

                    b.ToTable("LeaveRequests");
                });

            modelBuilder.Entity("Domain.Entities.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Domain.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<bool>("HasNewBorn")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsMarried")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("YearsOfService")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Domain.Entities.Tenant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("Domain.Rules.LeaveRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CountryId")
                        .HasColumnType("integer");

                    b.Property<string>("Expression")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FailureEvent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RuleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SuccessEvent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("TenantId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("TenantId");

                    b.ToTable("LeaveRules");
                });

            modelBuilder.Entity("Domain.DTOs.LeaveRequest", b =>
                {
                    b.HasOne("Domain.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Employee", null)
                        .WithMany("LeaveRequests")
                        .HasForeignKey("EmployeeId1");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Domain.Rules.LeaveRule", b =>
                {
                    b.HasOne("Domain.Entities.Country", "Country")
                        .WithMany("LeaveRules")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Tenant", "Tenant")
                        .WithMany("LeaveRules")
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Country");

                    b.Navigation("Tenant");
                });

            modelBuilder.Entity("Domain.Entities.Country", b =>
                {
                    b.Navigation("LeaveRules");
                });

            modelBuilder.Entity("Domain.Entities.Employee", b =>
                {
                    b.Navigation("LeaveRequests");
                });

            modelBuilder.Entity("Domain.Entities.Tenant", b =>
                {
                    b.Navigation("LeaveRules");
                });
#pragma warning restore 612, 618
        }
    }
}
