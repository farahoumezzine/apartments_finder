﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using miniprojet.Data;

#nullable disable

namespace miniprojet.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("miniprojet.Models.Appartement", b =>
                {
                    b.Property<int>("NumApp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IdProp")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Localite")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("NbrPièces")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Valeur")
                        .HasColumnType("TEXT");

                    b.HasKey("NumApp");

                    b.HasIndex("IdProp");

                    b.ToTable("Appartements");
                });

            modelBuilder.Entity("miniprojet.Models.Compte", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue("User");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Comptes");
                });

            modelBuilder.Entity("miniprojet.Models.Locataire", b =>
                {
                    b.Property<int>("IdLoc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Prénom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("IdLoc");

                    b.ToTable("Locataires");
                });

            modelBuilder.Entity("miniprojet.Models.Location", b =>
                {
                    b.Property<int>("IdLoc")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DatLoc")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Montant")
                        .HasColumnType("TEXT");

                    b.Property<int>("NbrMois")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumApp")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdLoc");

                    b.HasIndex("NumApp");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("miniprojet.Models.Propriétaire", b =>
                {
                    b.Property<int>("IdProp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Prénom")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IdProp");

                    b.ToTable("Propriétaires");
                });

            modelBuilder.Entity("miniprojet.Models.Appartement", b =>
                {
                    b.HasOne("miniprojet.Models.Propriétaire", "Propriétaire")
                        .WithMany("Appartements")
                        .HasForeignKey("IdProp")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Propriétaire");
                });

            modelBuilder.Entity("miniprojet.Models.Location", b =>
                {
                    b.HasOne("miniprojet.Models.Locataire", "Locataire")
                        .WithMany("Locations")
                        .HasForeignKey("IdLoc")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("miniprojet.Models.Appartement", "Appartement")
                        .WithMany()
                        .HasForeignKey("NumApp")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Appartement");

                    b.Navigation("Locataire");
                });

            modelBuilder.Entity("miniprojet.Models.Locataire", b =>
                {
                    b.Navigation("Locations");
                });

            modelBuilder.Entity("miniprojet.Models.Propriétaire", b =>
                {
                    b.Navigation("Appartements");
                });
#pragma warning restore 612, 618
        }
    }
}
