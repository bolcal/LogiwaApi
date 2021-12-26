﻿// <auto-generated />
using System;
using LogiwaApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LogiwaApi.Migrations
{
    [DbContext(typeof(LgwDbContext))]
    partial class LgwDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("LogiwaApi.Data.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("MinStockToLive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            MinStockToLive = 10,
                            Name = "Headphones"
                        },
                        new
                        {
                            Id = 2,
                            MinStockToLive = 50,
                            Name = "Mouse"
                        },
                        new
                        {
                            Id = 3,
                            MinStockToLive = 10,
                            Name = "Keyboard"
                        },
                        new
                        {
                            Id = 4,
                            MinStockToLive = 10,
                            Name = "Monitor"
                        });
                });

            modelBuilder.Entity("LogiwaApi.Data.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StockQuantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Description = "Bluetooth headphones with noise cancelling In-Ear",
                            StockQuantity = 20,
                            Title = "Sony WF-1000"
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 1,
                            Description = "Over-the-Ear Headphones",
                            StockQuantity = 40,
                            Title = "Phillips T90"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Wired Mouse Optical Tracking",
                            StockQuantity = 100,
                            Title = "Logitech M150"
                        },
                        new
                        {
                            Id = 4,
                            CategoryId = 3,
                            StockQuantity = 4,
                            Title = "Logitech Internet Pro"
                        },
                        new
                        {
                            Id = 5,
                            CategoryId = 3,
                            StockQuantity = 8,
                            Title = "Asus Rog"
                        },
                        new
                        {
                            Id = 6,
                            CategoryId = 4,
                            Description = "Ultrawide monitor 75hz Full HD",
                            StockQuantity = 50,
                            Title = "LG 29WL500"
                        },
                        new
                        {
                            Id = 7,
                            CategoryId = 2,
                            Description = "Wired Gaming Mouse Optical Tracking 8000 DPI ",
                            StockQuantity = 30,
                            Title = "Logitech G502"
                        },
                        new
                        {
                            Id = 8,
                            CategoryId = 2,
                            Description = "Optical Wireless Gaming Mouse",
                            StockQuantity = 55,
                            Title = "Msi GG GM08"
                        },
                        new
                        {
                            Id = 9,
                            CategoryId = 4,
                            Description = "IPS 165Hz HDR Gaming Monitor",
                            StockQuantity = 25,
                            Title = "HP X34"
                        });
                });

            modelBuilder.Entity("LogiwaApi.Data.Entities.Product", b =>
                {
                    b.HasOne("LogiwaApi.Data.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}