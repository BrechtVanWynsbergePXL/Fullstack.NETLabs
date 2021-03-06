// <auto-generated />
using System;
using DevOps.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DevOps.Infrastructure.Migrations
{
    [DbContext(typeof(DevOpsContext))]
    partial class DevOpsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DevOps.Domain.Developer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Rating_value")
                        .HasColumnType("float");

                    b.Property<Guid?>("TeamId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Rating_value");

                    b.HasIndex("TeamId");

                    b.ToTable("Developers");
                });

            modelBuilder.Entity("DevOps.Domain.Percentage", b =>
                {
                    b.Property<double>("_value")
                        .HasColumnType("float");

                    b.HasKey("_value");

                    b.ToTable("Percentage");
                });

            modelBuilder.Entity("Team", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("DevOps.Domain.Developer", b =>
                {
                    b.HasOne("DevOps.Domain.Percentage", "Rating")
                        .WithMany()
                        .HasForeignKey("Rating_value");

                    b.HasOne("Team", null)
                        .WithMany("Developers")
                        .HasForeignKey("TeamId");

                    b.Navigation("Rating");
                });

            modelBuilder.Entity("Team", b =>
                {
                    b.Navigation("Developers");
                });
#pragma warning restore 612, 618
        }
    }
}
