﻿// <auto-generated />
using Intranet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Intranet.Migrations.InvEmail
{
    [DbContext(typeof(InvEmailContext))]
    partial class InvEmailContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Intranet.Models.InvEmail", b =>
                {
                    b.Property<int>("InvEmailId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("InvEmailAddress")
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

                    b.HasKey("InvEmailId");

                    b.ToTable("invEmails");
                });
#pragma warning restore 612, 618
        }
    }
}
