﻿// <auto-generated />
using Intranet.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Intranet.DataAccess.Data.Migrations.CorpComm
{
    [DbContext(typeof(CorpCommDbContext))]
    [Migration("20200414004132_AddDbToSQL")]
    partial class AddDbToSQL
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Intranet.Models.CorpComm.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UserDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("UserIP")
                        .IsRequired()
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("Intranet.Models.CorpComm.StationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("UserIP")
                        .IsRequired()
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.ToTable("StationTypes");
                });

            modelBuilder.Entity("Intranet.Models.CorpComm.Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("UserIP")
                        .IsRequired()
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.ToTable("Units");
                });
#pragma warning restore 612, 618
        }
    }
}
