﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheDialgaTeam.Discord.Bot.Services.EntityFramework;

namespace TheDialgaTeam.Discord.Bot.Migrations
{
    [DbContext(typeof(SqliteContext))]
    partial class SqliteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024");

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordApp", b =>
                {
                    b.Property<int>("DiscordAppId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppDescription")
                        .HasMaxLength(400);

                    b.Property<string>("AppName")
                        .HasMaxLength(32);

                    b.Property<string>("BotToken")
                        .IsRequired()
                        .HasMaxLength(59);

                    b.Property<ulong>("ClientId");

                    b.Property<string>("ClientSecret")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<DateTimeOffset?>("LastUpdateCheck");

                    b.HasKey("DiscordAppId");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("DiscordAppTable");
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordAppChannel", b =>
                {
                    b.Property<int>("DiscordAppChannelId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DiscordAppGuildId");

                    b.Property<int>("DiscordChannelId");

                    b.HasKey("DiscordAppChannelId");

                    b.HasIndex("DiscordAppGuildId");

                    b.HasIndex("DiscordChannelId");

                    b.ToTable("DiscordAppChannelTable");
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordAppGlobalOwner", b =>
                {
                    b.Property<int>("DiscordAppGlobalOwnerId")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("UserId");

                    b.HasKey("DiscordAppGlobalOwnerId");

                    b.ToTable("DiscordAppGlobalOwnerTable");
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordAppGuild", b =>
                {
                    b.Property<int>("DiscordAppGuildId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("DeleteCommandAfterUse");

                    b.Property<int>("DiscordAppId");

                    b.Property<int>("DiscordGuildId");

                    b.Property<string>("Prefix")
                        .HasMaxLength(255);

                    b.HasKey("DiscordAppGuildId");

                    b.HasIndex("DiscordAppId");

                    b.HasIndex("DiscordGuildId");

                    b.ToTable("DiscordAppGuildTable");
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordAppGuildModule", b =>
                {
                    b.Property<int>("DiscordAppGuildModuleId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("DiscordAppGuildId");

                    b.Property<int>("DiscordModuleId");

                    b.HasKey("DiscordAppGuildModuleId");

                    b.HasIndex("DiscordAppGuildId");

                    b.HasIndex("DiscordModuleId");

                    b.ToTable("DiscordAppGuildModuleTable");
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordAppOwner", b =>
                {
                    b.Property<int>("DiscordAppOwnerId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DiscordAppId");

                    b.Property<ulong>("UserId");

                    b.HasKey("DiscordAppOwnerId");

                    b.HasIndex("DiscordAppId");

                    b.ToTable("DiscordAppOwnerTable");
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordChannel", b =>
                {
                    b.Property<int>("DiscordChannelId")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("ChannelId");

                    b.Property<int>("DiscordGuildId");

                    b.HasKey("DiscordChannelId");

                    b.HasIndex("ChannelId")
                        .IsUnique();

                    b.HasIndex("DiscordGuildId");

                    b.ToTable("DiscordChannelTable");
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordChannelModerator", b =>
                {
                    b.Property<int>("DiscordChannelModeratorId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DiscordChannelId");

                    b.Property<int>("Type");

                    b.Property<ulong>("Value");

                    b.HasKey("DiscordChannelModeratorId");

                    b.HasIndex("DiscordChannelId");

                    b.ToTable("DiscordChannelModeratorTable");
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordGuild", b =>
                {
                    b.Property<int>("DiscordGuildId")
                        .ValueGeneratedOnAdd();

                    b.Property<ulong>("GuildId");

                    b.HasKey("DiscordGuildId");

                    b.HasIndex("GuildId")
                        .IsUnique();

                    b.ToTable("DiscordGuildTable");
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordGuildModerator", b =>
                {
                    b.Property<int>("DiscordGuildModeratorId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DiscordGuildId");

                    b.Property<int>("Type");

                    b.Property<ulong>("Value");

                    b.HasKey("DiscordGuildModeratorId");

                    b.HasIndex("DiscordGuildId");

                    b.ToTable("DiscordGuildModeratorTable");
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordModule", b =>
                {
                    b.Property<int>("DiscordModuleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Module")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("DiscordModuleId");

                    b.ToTable("DiscordModuleTable");
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordModuleRequirement", b =>
                {
                    b.Property<int>("DiscordModuleRequirementId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DiscordGuildId");

                    b.Property<int>("DiscordModuleId");

                    b.HasKey("DiscordModuleRequirementId");

                    b.HasIndex("DiscordGuildId");

                    b.HasIndex("DiscordModuleId");

                    b.ToTable("DiscordModuleRequirementTable");
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.FreeGameNotification", b =>
                {
                    b.Property<int>("FreeGameNotificationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DiscordChannelId");

                    b.Property<ulong>("RoleId");

                    b.HasKey("FreeGameNotificationId");

                    b.HasIndex("DiscordChannelId")
                        .IsUnique();

                    b.ToTable("FreeGameNotificationTable");
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordAppChannel", b =>
                {
                    b.HasOne("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordAppGuild", "DiscordAppGuild")
                        .WithMany("DiscordAppChannels")
                        .HasForeignKey("DiscordAppGuildId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordChannel", "DiscordChannel")
                        .WithMany("DiscordAppChannels")
                        .HasForeignKey("DiscordChannelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordAppGuild", b =>
                {
                    b.HasOne("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordApp", "DiscordApp")
                        .WithMany("DiscordAppGuilds")
                        .HasForeignKey("DiscordAppId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordGuild", "DiscordGuild")
                        .WithMany("DiscordAppGuilds")
                        .HasForeignKey("DiscordGuildId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordAppGuildModule", b =>
                {
                    b.HasOne("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordAppGuild", "DiscordAppGuild")
                        .WithMany("DiscordAppGuildModules")
                        .HasForeignKey("DiscordAppGuildId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordModule", "DiscordModule")
                        .WithMany("DiscordAppGuildModules")
                        .HasForeignKey("DiscordModuleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordAppOwner", b =>
                {
                    b.HasOne("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordApp", "DiscordApp")
                        .WithMany("DiscordAppOwners")
                        .HasForeignKey("DiscordAppId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordChannel", b =>
                {
                    b.HasOne("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordGuild", "DiscordGuild")
                        .WithMany("DiscordChannels")
                        .HasForeignKey("DiscordGuildId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordChannelModerator", b =>
                {
                    b.HasOne("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordChannel", "DiscordChannel")
                        .WithMany("DiscordChannelModerators")
                        .HasForeignKey("DiscordChannelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordGuildModerator", b =>
                {
                    b.HasOne("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordGuild", "DiscordGuild")
                        .WithMany("DiscordGuildModerators")
                        .HasForeignKey("DiscordGuildId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordModuleRequirement", b =>
                {
                    b.HasOne("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordGuild", "DiscordGuild")
                        .WithMany("DiscordModuleRequirements")
                        .HasForeignKey("DiscordGuildId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordModule", "DiscordModule")
                        .WithMany("DiscordModuleRequirements")
                        .HasForeignKey("DiscordModuleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TheDialgaTeam.Discord.Bot.Models.EntityFramework.FreeGameNotification", b =>
                {
                    b.HasOne("TheDialgaTeam.Discord.Bot.Models.EntityFramework.DiscordChannel", "DiscordChannel")
                        .WithOne("FreeGameNotification")
                        .HasForeignKey("TheDialgaTeam.Discord.Bot.Models.EntityFramework.FreeGameNotification", "DiscordChannelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
