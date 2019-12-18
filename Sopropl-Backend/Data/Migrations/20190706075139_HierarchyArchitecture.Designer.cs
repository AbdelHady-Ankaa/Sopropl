﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sopropl_Backend.Data;

namespace Sopropl_Backend.Data.Migrations
{
    [DbContext(typeof(SoproplDbContext))]
    [Migration("20190706075139_HierarchyArchitecture")]
    partial class HierarchyArchitecture
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sopropl_Backend.Models.Access", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NormalizedOrganizationName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedProjectName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedTeamName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<short>("Permission");

                    b.HasKey("Id");

                    b.HasAlternateKey("NormalizedOrganizationName", "NormalizedProjectName", "NormalizedTeamName");

                    b.HasIndex("NormalizedProjectName", "NormalizedOrganizationName");

                    b.HasIndex("NormalizedTeamName", "NormalizedOrganizationName");

                    b.ToTable("AccessList");
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Activity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Description")
                        .HasColumnType("ntext");

                    b.Property<double>("Duration");

                    b.Property<DateTime?>("EarlyFinish");

                    b.Property<DateTime?>("EarlyStart");

                    b.Property<double>("EstimatedHours");

                    b.Property<double>("FreeFloat");

                    b.Property<double>("HoursSpent");

                    b.Property<DateTime?>("LateFinish");

                    b.Property<DateTime?>("LateStart");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedOrganizationName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedProjectName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("OrganizationId");

                    b.Property<byte>("Priority");

                    b.Property<int>("State");

                    b.Property<double>("TotalFloat");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("NormalizedProjectName", "NormalizedOrganizationName");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Arrow", b =>
                {
                    b.Property<string>("NormalizedProjectName")
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedOrganizationName")
                        .HasMaxLength(128);

                    b.Property<string>("FromActivityName");

                    b.Property<string>("ToActivityName");

                    b.Property<double>("ConstraintValue");

                    b.Property<int>("Type");

                    b.HasKey("NormalizedProjectName", "NormalizedOrganizationName", "FromActivityName", "ToActivityName");

                    b.HasIndex("FromActivityName", "NormalizedProjectName", "NormalizedOrganizationName");

                    b.HasIndex("ToActivityName", "NormalizedProjectName", "NormalizedOrganizationName");

                    b.ToTable("Arrows");
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Invitation", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NormalizedOrganizationName")
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedOrganizationName");

                    b.HasIndex("NormalizedUserName");

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Member", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<short>("MemberType");

                    b.Property<string>("NormalizedOrganizationName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedTeamName")
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedUserName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedUserName");

                    b.HasIndex("NormalizedOrganizationName", "NormalizedTeamName");

                    b.HasIndex("NormalizedOrganizationName", "NormalizedUserName", "NormalizedTeamName")
                        .IsUnique()
                        .HasFilter("[NormalizedOrganizationName] IS NOT NULL AND [NormalizedUserName] IS NOT NULL AND [NormalizedTeamName] IS NOT NULL");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Notification", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<string>("Icon");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(128);

                    b.Property<string>("Title");

                    b.Property<DateTime>("sentDate");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedUserName");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Organization", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("ContactPhone");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsBeta");

                    b.Property<bool>("IsDeactivated");

                    b.Property<string>("LogoId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Website");

                    b.HasKey("Id");

                    b.HasIndex("LogoId")
                        .IsUnique()
                        .HasFilter("[LogoId] IS NOT NULL");

                    b.HasIndex("NormalizedName")
                        .IsUnique();

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Photo", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("PublicId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Project", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("Description")
                        .HasColumnType("ntext");

                    b.Property<bool>("IsActive");

                    b.Property<string>("LogoId");

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedOrganizationName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("ProjectType");

                    b.Property<string>("ShortName");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("LogoId")
                        .IsUnique()
                        .HasFilter("[LogoId] IS NOT NULL");

                    b.HasIndex("NormalizedOrganizationName");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Resource", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActivityName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("MemberId");

                    b.Property<string>("NormalizedOrganizationName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedProjectName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedTeamName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasAlternateKey("ActivityName", "NormalizedTeamName", "NormalizedOrganizationName", "NormalizedProjectName");

                    b.HasIndex("MemberId");

                    b.HasIndex("NormalizedTeamName", "NormalizedOrganizationName");

                    b.HasIndex("ActivityName", "NormalizedProjectName", "NormalizedOrganizationName");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Team", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("NormalizedOrganizationName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Sopropl_Backend.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Address");

                    b.Property<string>("Bio");

                    b.Property<string>("City");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Country");

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<DateTime>("LastActive");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("PhotoId");

                    b.Property<string>("PostalCode");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("PhotoId")
                        .IsUnique()
                        .HasFilter("[PhotoId] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Access", b =>
                {
                    b.HasOne("Sopropl_Backend.Models.Project", "Project")
                        .WithMany("AccessList")
                        .HasForeignKey("NormalizedProjectName", "NormalizedOrganizationName")
                        .HasPrincipalKey("NormalizedName", "NormalizedOrganizationName")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Sopropl_Backend.Models.Team", "Team")
                        .WithMany("AccessList")
                        .HasForeignKey("NormalizedTeamName", "NormalizedOrganizationName")
                        .HasPrincipalKey("NormalizedName", "NormalizedOrganizationName")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Activity", b =>
                {
                    b.HasOne("Sopropl_Backend.Models.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId");

                    b.HasOne("Sopropl_Backend.Models.Project", "Project")
                        .WithMany("Activities")
                        .HasForeignKey("NormalizedProjectName", "NormalizedOrganizationName")
                        .HasPrincipalKey("NormalizedName", "NormalizedOrganizationName")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Arrow", b =>
                {
                    b.HasOne("Sopropl_Backend.Models.Activity", "ToActivity")
                        .WithMany("InArrows")
                        .HasForeignKey("FromActivityName", "NormalizedProjectName", "NormalizedOrganizationName")
                        .HasPrincipalKey("Name", "NormalizedProjectName", "NormalizedOrganizationName")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Sopropl_Backend.Models.Activity", "FromActivity")
                        .WithMany("OutArrows")
                        .HasForeignKey("ToActivityName", "NormalizedProjectName", "NormalizedOrganizationName")
                        .HasPrincipalKey("Name", "NormalizedProjectName", "NormalizedOrganizationName")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Invitation", b =>
                {
                    b.HasOne("Sopropl_Backend.Models.Organization", "Organization")
                        .WithMany("Invitations")
                        .HasForeignKey("NormalizedOrganizationName")
                        .HasPrincipalKey("NormalizedName")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sopropl_Backend.Models.User", "User")
                        .WithMany("Invitations")
                        .HasForeignKey("NormalizedUserName")
                        .HasPrincipalKey("NormalizedUserName")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Member", b =>
                {
                    b.HasOne("Sopropl_Backend.Models.Organization", "Organiztion")
                        .WithMany("Members")
                        .HasForeignKey("NormalizedOrganizationName")
                        .HasPrincipalKey("NormalizedName")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sopropl_Backend.Models.User", "User")
                        .WithMany("Members")
                        .HasForeignKey("NormalizedUserName")
                        .HasPrincipalKey("NormalizedUserName")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sopropl_Backend.Models.Team", "Team")
                        .WithMany("Members")
                        .HasForeignKey("NormalizedOrganizationName", "NormalizedTeamName")
                        .HasPrincipalKey("NormalizedOrganizationName", "NormalizedName")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Notification", b =>
                {
                    b.HasOne("Sopropl_Backend.Models.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("NormalizedUserName")
                        .HasPrincipalKey("NormalizedUserName")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Organization", b =>
                {
                    b.HasOne("Sopropl_Backend.Models.Photo", "Logo")
                        .WithOne("Organization")
                        .HasForeignKey("Sopropl_Backend.Models.Organization", "LogoId");
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Project", b =>
                {
                    b.HasOne("Sopropl_Backend.Models.User", "Client")
                        .WithMany("Projects")
                        .HasForeignKey("ClientId");

                    b.HasOne("Sopropl_Backend.Models.Photo", "Logo")
                        .WithOne("Project")
                        .HasForeignKey("Sopropl_Backend.Models.Project", "LogoId");

                    b.HasOne("Sopropl_Backend.Models.Organization", "Organization")
                        .WithMany("Projects")
                        .HasForeignKey("NormalizedOrganizationName")
                        .HasPrincipalKey("NormalizedName")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Resource", b =>
                {
                    b.HasOne("Sopropl_Backend.Models.Member")
                        .WithMany("Tasks")
                        .HasForeignKey("MemberId");

                    b.HasOne("Sopropl_Backend.Models.Team", "Team")
                        .WithMany("Tasks")
                        .HasForeignKey("NormalizedTeamName", "NormalizedOrganizationName")
                        .HasPrincipalKey("NormalizedName", "NormalizedOrganizationName")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sopropl_Backend.Models.Activity", "Activity")
                        .WithMany("Resources")
                        .HasForeignKey("ActivityName", "NormalizedProjectName", "NormalizedOrganizationName")
                        .HasPrincipalKey("Name", "NormalizedProjectName", "NormalizedOrganizationName")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Sopropl_Backend.Models.Team", b =>
                {
                    b.HasOne("Sopropl_Backend.Models.Organization", "Organization")
                        .WithMany("Teams")
                        .HasForeignKey("NormalizedOrganizationName")
                        .HasPrincipalKey("NormalizedName")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sopropl_Backend.Models.User", b =>
                {
                    b.HasOne("Sopropl_Backend.Models.Photo", "Photo")
                        .WithOne("User")
                        .HasForeignKey("Sopropl_Backend.Models.User", "PhotoId")
                        .OnDelete(DeleteBehavior.SetNull);
                });
#pragma warning restore 612, 618
        }
    }
}
