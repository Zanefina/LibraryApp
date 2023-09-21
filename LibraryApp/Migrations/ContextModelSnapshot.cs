﻿// <auto-generated />
using System;
using LibraryApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LibraryApp.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LibraryApp.Models.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookId"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Availability")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PublishedOn")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BookId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            BookId = 1,
                            Author = "Frances Hodgson Burnett",
                            Availability = true,
                            Description = "A timeless classic about the power of nature and friendship.",
                            Genre = "Children's Fiction",
                            PublishedOn = new DateTime(1911, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "The Secret Garden"
                        },
                        new
                        {
                            BookId = 2,
                            Author = "Harper Lee",
                            Availability = false,
                            Description = "A compelling story of racial injustice and moral growth in the American South.",
                            Genre = "Fiction",
                            PublishedOn = new DateTime(1960, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "To Kill a Mockingbird"
                        },
                        new
                        {
                            BookId = 3,
                            Author = "F. Scott Fitzgerald",
                            Availability = true,
                            Description = "A tale of decadence, obsession, and the American Dream during the Roaring Twenties.",
                            Genre = "Classic Literature",
                            PublishedOn = new DateTime(1925, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "The Great Gatsby"
                        },
                        new
                        {
                            BookId = 4,
                            Author = "J.R.R. Tolkien",
                            Availability = false,
                            Description = "The adventurous journey of Bilbo Baggins, a hobbit, to reclaim treasure guarded by a dragon.",
                            Genre = "Fantasy",
                            PublishedOn = new DateTime(1937, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "The Hobbit"
                        },
                        new
                        {
                            BookId = 5,
                            Author = "J.D. Salinger",
                            Availability = true,
                            Description = "Holden Caulfield's existential journey through the streets of New York City.",
                            Genre = "Coming-of-Age",
                            PublishedOn = new DateTime(1951, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "The Catcher in the Rye"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}