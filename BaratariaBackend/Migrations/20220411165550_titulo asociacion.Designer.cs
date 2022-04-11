﻿// <auto-generated />
using System;
using BaratariaBackend.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BaratariaBackend.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220411165550_titulo asociacion")]
    partial class tituloasociacion
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("BaratariaBackend.Models.Entities.Actividad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("FechaAlta")
                        .HasColumnType("timestamp");

                    b.Property<DateTime?>("FechaFin")
                        .HasColumnType("timestamp");

                    b.Property<DateTime?>("FechaInicio")
                        .HasColumnType("timestamp");

                    b.Property<string>("ImagenOriginal")
                        .HasColumnType("varchar(200)");

                    b.Property<long?>("ImagenPeso")
                        .HasColumnType("bigint");

                    b.Property<string>("ImagenServidor")
                        .HasColumnType("varchar(200)");

                    b.Property<bool?>("Mostrar")
                        .HasColumnType("boolean");

                    b.Property<string>("Texto")
                        .HasColumnType("varchar");

                    b.Property<string>("Titulo")
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Actividades");
                });

            modelBuilder.Entity("BaratariaBackend.Models.Entities.Asociacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("FechaAlta")
                        .HasColumnType("timestamp");

                    b.Property<string>("Texto")
                        .HasColumnType("varchar");

                    b.Property<string>("Titulo")
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Asociacions");
                });

            modelBuilder.Entity("BaratariaBackend.Models.Entities.Convenio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("FechaAlta")
                        .HasColumnType("timestamp");

                    b.Property<string>("ImagenOriginal")
                        .HasColumnType("varchar(200)");

                    b.Property<long?>("ImagenPeso")
                        .HasColumnType("bigint");

                    b.Property<string>("ImagenServidor")
                        .HasColumnType("varchar(200)");

                    b.Property<bool?>("Mostrar")
                        .HasColumnType("boolean");

                    b.Property<string>("Texto")
                        .HasColumnType("varchar");

                    b.Property<string>("Titulo")
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Convenios");
                });

            modelBuilder.Entity("BaratariaBackend.Models.Entities.DireccionWeb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ActividadId")
                        .HasColumnType("integer");

                    b.Property<int?>("ConvenioId")
                        .HasColumnType("integer");

                    b.Property<string>("Nombre")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Url")
                        .HasColumnType("varchar");

                    b.HasKey("Id");

                    b.HasIndex("ActividadId");

                    b.HasIndex("ConvenioId");

                    b.ToTable("DireccionWebs");
                });

            modelBuilder.Entity("BaratariaBackend.Models.Entities.Documento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ActividadId")
                        .HasColumnType("integer");

                    b.Property<int?>("AsociacionId")
                        .HasColumnType("integer");

                    b.Property<int?>("ConvenioId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("Fecha")
                        .HasColumnType("timestamp");

                    b.Property<string>("Nombre")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Original")
                        .HasColumnType("varchar(200)");

                    b.Property<bool?>("Privado")
                        .HasColumnType("boolean");

                    b.Property<string>("Servidor")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Tamanio")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Url")
                        .HasColumnType("varchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("ActividadId");

                    b.HasIndex("AsociacionId");

                    b.HasIndex("ConvenioId");

                    b.ToTable("Documentos");
                });

            modelBuilder.Entity("BaratariaBackend.Models.Entities.Enlace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("FechaAlta")
                        .HasColumnType("timestamp");

                    b.Property<string>("ImagenOriginal")
                        .HasColumnType("varchar");

                    b.Property<long?>("ImagenPeso")
                        .HasColumnType("bigint");

                    b.Property<string>("ImagenServidor")
                        .HasColumnType("varchar(200)");

                    b.Property<bool?>("Mostrar")
                        .HasColumnType("boolean");

                    b.Property<string>("Titulo")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Url")
                        .HasColumnType("varchar");

                    b.HasKey("Id");

                    b.ToTable("Enlaces");
                });

            modelBuilder.Entity("BaratariaBackend.Models.Entities.DireccionWeb", b =>
                {
                    b.HasOne("BaratariaBackend.Models.Entities.Actividad", null)
                        .WithMany("DireccionWebs")
                        .HasForeignKey("ActividadId");

                    b.HasOne("BaratariaBackend.Models.Entities.Convenio", null)
                        .WithMany("DireccionWebs")
                        .HasForeignKey("ConvenioId");
                });

            modelBuilder.Entity("BaratariaBackend.Models.Entities.Documento", b =>
                {
                    b.HasOne("BaratariaBackend.Models.Entities.Actividad", null)
                        .WithMany("Documentos")
                        .HasForeignKey("ActividadId");

                    b.HasOne("BaratariaBackend.Models.Entities.Asociacion", null)
                        .WithMany("Documentos")
                        .HasForeignKey("AsociacionId");

                    b.HasOne("BaratariaBackend.Models.Entities.Convenio", null)
                        .WithMany("Documentos")
                        .HasForeignKey("ConvenioId");
                });

            modelBuilder.Entity("BaratariaBackend.Models.Entities.Actividad", b =>
                {
                    b.Navigation("DireccionWebs");

                    b.Navigation("Documentos");
                });

            modelBuilder.Entity("BaratariaBackend.Models.Entities.Asociacion", b =>
                {
                    b.Navigation("Documentos");
                });

            modelBuilder.Entity("BaratariaBackend.Models.Entities.Convenio", b =>
                {
                    b.Navigation("DireccionWebs");

                    b.Navigation("Documentos");
                });
#pragma warning restore 612, 618
        }
    }
}
