﻿// <auto-generated />
using AudioPlayerProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AudioPlayerProject.Migrations.Playlist
{
    [DbContext(typeof(PlaylistContext))]
    partial class PlaylistContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("AudioPlayerProject.Models.Music", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Artist")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Music");
                });

            modelBuilder.Entity("AudioPlayerProject.Models.Playlist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("AudioPlayerProjectUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Playlist");
                });

            modelBuilder.Entity("AudioPlayerProject.Models.PlaylistMusic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("MusicId")
                        .HasColumnType("int");

                    b.Property<int>("PlaylistId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MusicId");

                    b.HasIndex("PlaylistId");

                    b.ToTable("PlaylistMusic");
                });

            modelBuilder.Entity("AudioPlayerProject.Models.PlaylistMusic", b =>
                {
                    b.HasOne("AudioPlayerProject.Models.Music", "Music")
                        .WithMany("PlaylistMusics")
                        .HasForeignKey("MusicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AudioPlayerProject.Models.Playlist", "Playlist")
                        .WithMany("PlaylistMusics")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Music");

                    b.Navigation("Playlist");
                });

            modelBuilder.Entity("AudioPlayerProject.Models.Music", b =>
                {
                    b.Navigation("PlaylistMusics");
                });

            modelBuilder.Entity("AudioPlayerProject.Models.Playlist", b =>
                {
                    b.Navigation("PlaylistMusics");
                });
#pragma warning restore 612, 618
        }
    }
}
