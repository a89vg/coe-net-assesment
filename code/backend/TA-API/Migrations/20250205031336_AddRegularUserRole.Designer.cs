﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TA_API.Services.Data;

#nullable disable

namespace TA_API.Migrations
{
    [DbContext(typeof(AssessmentDbContext))]
    [Migration("20250205031336_AddRegularUserRole")]
    partial class AddRegularUserRole
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("TA_API.Models.Data.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            Name = "User"
                        });
                });

            modelBuilder.Entity("TA_API.Models.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateOfBirth = new DateTime(1989, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "admin@unosquare.com",
                            FullName = "Alejandro Vázquez Góngora",
                            PasswordHash = "AQAAAAIAAYagAAAAEPqCWJ7ya9WKGosM3zc8YI8gLojDSX+psFE9t73BstHwFT5XwOFFlTQwC65YvoK0sQ==",
                            Username = "admin"
                        },
                        new
                        {
                            Id = 2,
                            DateOfBirth = new DateTime(1989, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "alejandro.vazquez@unosquare.com",
                            FullName = "Alejandro Vázquez Gógnora",
                            PasswordHash = "AQAAAAIAAYagAAAAEEOCuWEdhDbhYbrCSiQmdXZG9ZZHq1bcDo4Ku5klEDg8TkTpma+DciN19NO8dzlA6w==",
                            Username = "avazquez"
                        });
                });

            modelBuilder.Entity("TA_API.Models.Data.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Username");

                    b.ToTable("UserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Role = "Admin",
                            Username = "admin"
                        },
                        new
                        {
                            Id = 2,
                            Role = "User",
                            Username = "avazquez"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
