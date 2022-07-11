﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieRestful.Repository;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MovieRestful.Repository.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20220711131149_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
#pragma warning restore 612, 618
        }
    }
}
