﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieRestful.Repository;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MovieRestful.Repository.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MovieRestful.Core.Models.Movie", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("id"));

                    b.Property<int>("ViewedMovieCount")
                        .HasColumnType("integer");

                    b.Property<string>("adult")
                        .HasColumnType("text");

                    b.Property<string>("belongs_to_collection")
                        .HasColumnType("text");

                    b.Property<string>("budget")
                        .HasColumnType("text");

                    b.Property<string>("genres")
                        .HasColumnType("text");

                    b.Property<string>("homepage")
                        .HasColumnType("text");

                    b.Property<string>("imdb_id")
                        .HasColumnType("text");

                    b.Property<string>("original_language")
                        .HasColumnType("text");

                    b.Property<string>("original_title")
                        .HasColumnType("text");

                    b.Property<string>("overview")
                        .HasColumnType("text");

                    b.Property<string>("popularity")
                        .HasColumnType("text");

                    b.Property<string>("poster_path")
                        .HasColumnType("text");

                    b.Property<string>("production_companies")
                        .HasColumnType("text");

                    b.Property<string>("production_countries")
                        .HasColumnType("text");

                    b.Property<DateTime?>("release_date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("revenue")
                        .HasColumnType("integer");

                    b.Property<decimal?>("runtime")
                        .HasColumnType("numeric");

                    b.Property<string>("spoken_languages")
                        .HasColumnType("text");

                    b.Property<string>("status")
                        .HasColumnType("text");

                    b.Property<string>("tagline")
                        .HasColumnType("text");

                    b.Property<string>("title")
                        .HasColumnType("text");

                    b.Property<string>("video")
                        .HasColumnType("text");

                    b.Property<decimal?>("vote_average")
                        .HasColumnType("numeric");

                    b.Property<int?>("vote_count")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("mytable");
                });

            modelBuilder.Entity("MovieRestful.Core.Models.Trending", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("id"));

                    b.Property<long>("MovieId")
                        .HasColumnType("bigint");

                    b.Property<int>("ViewedMovieCount")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.HasIndex("MovieId")
                        .IsUnique();

                    b.ToTable("Trendings");
                });

            modelBuilder.Entity("MovieRestful.Core.Models.Trending", b =>
                {
                    b.HasOne("MovieRestful.Core.Models.Movie", "Movie")
                        .WithOne("Trending")
                        .HasForeignKey("MovieRestful.Core.Models.Trending", "MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("MovieRestful.Core.Models.Movie", b =>
                {
                    b.Navigation("Trending")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
