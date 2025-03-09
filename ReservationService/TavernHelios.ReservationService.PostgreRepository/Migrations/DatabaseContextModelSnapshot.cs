﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TavernHelios.ReservationService.PostgreRepository;

#nullable disable

namespace TavernHelios.ReservationService.PostgreRepository.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TavernHelios.ReservationService.APICore.Entities.DishReservationEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("DishId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<long>("ReservationId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ReservationId");

                    b.ToTable("DishReservationEntity");
                });

            modelBuilder.Entity("TavernHelios.ReservationService.APICore.Entities.ReservationEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("PersonId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("ReservationEntity");
                });

            modelBuilder.Entity("TavernHelios.ReservationService.APICore.Entities.DishReservationEntity", b =>
                {
                    b.HasOne("TavernHelios.ReservationService.APICore.Entities.ReservationEntity", "Reservation")
                        .WithMany("DishReservations")
                        .HasForeignKey("ReservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("TavernHelios.ReservationService.APICore.Entities.ReservationEntity", b =>
                {
                    b.Navigation("DishReservations");
                });
#pragma warning restore 612, 618
        }
    }
}
