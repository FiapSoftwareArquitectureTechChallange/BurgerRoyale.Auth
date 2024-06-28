﻿// <auto-generated />
using System;
using BurgerRoyale.Auth.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BurgerRoyale.Auth.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240624225008_AddedUsersInfo")]
    partial class AddedUsersInfo
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BurgerRoyale.Auth.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserRole")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("64121a27-126e-4143-a49c-10beb91eadc6"),
                            Address = "",
                            Cpf = "00000000000",
                            CreatedAt = new DateTime(2024, 6, 24, 22, 50, 8, 21, DateTimeKind.Utc).AddTicks(6292),
                            Email = "admin@burgerroyale.com",
                            Name = "Admin",
                            PasswordHash = "$2a$11$C99.K9/gfTc0RqR8XYAiu.T3BG/GvWgOt2oggKkyivz9dGpZPwpEy",
                            Phone = "",
                            UserRole = 0
                        },
                        new
                        {
                            Id = new Guid("446a4c3b-b34a-4aa0-ac0e-5d8743916b17"),
                            Address = "",
                            Cpf = "11111111111",
                            CreatedAt = new DateTime(2024, 6, 24, 22, 50, 8, 21, DateTimeKind.Utc).AddTicks(8348),
                            Email = "customer@burgerroyale.com",
                            Name = "Customer",
                            PasswordHash = "$2a$11$mDJa/xLGCCAYzxhDpcmYve8NpaBqMeCMMfVbB9NpGqg/SpaRv3gJq",
                            Phone = "",
                            UserRole = 1
                        },
                        new
                        {
                            Id = new Guid("f5977467-4883-4c6e-adb9-f6aff2fd5335"),
                            Address = "",
                            Cpf = "22222222222",
                            CreatedAt = new DateTime(2024, 6, 24, 22, 50, 8, 21, DateTimeKind.Utc).AddTicks(9578),
                            Email = "employee@burgerroyale.com",
                            Name = "Employee",
                            PasswordHash = "$2a$11$hIwenwL9SKoqgwWt0EOQgukwjCDP1tVuij0lMtHk9nGSwnIVbzbM2",
                            Phone = "",
                            UserRole = 2
                        });
                });
#pragma warning restore 612, 618
        }
    }
}