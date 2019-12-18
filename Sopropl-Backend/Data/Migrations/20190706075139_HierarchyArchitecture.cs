using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopropl_Backend.Data.Migrations
{
    public partial class HierarchyArchitecture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    PublicId = table.Column<string>(nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NormalizedName = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    LogoId = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    ContactPhone = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    IsBeta = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeactivated = table.Column<bool>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.UniqueConstraint("AK_Organizations_NormalizedName", x => x.NormalizedName);
                    table.ForeignKey(
                        name: "FK_Organizations_Photos_LogoId",
                        column: x => x.LogoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NormalizedUserName = table.Column<string>(maxLength: 128, nullable: false),
                    UserName = table.Column<string>(maxLength: 128, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Bio = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastActive = table.Column<DateTime>(nullable: false),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhotoId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_NormalizedUserName", x => x.NormalizedUserName);
                    table.ForeignKey(
                        name: "FK_Users_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NormalizedName = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    NormalizedOrganizationName = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.UniqueConstraint("AK_Teams_NormalizedName_NormalizedOrganizationName", x => new { x.NormalizedName, x.NormalizedOrganizationName });
                    table.UniqueConstraint("AK_Teams_NormalizedOrganizationName_NormalizedName", x => new { x.NormalizedOrganizationName, x.NormalizedName });
                    table.ForeignKey(
                        name: "FK_Teams_Organizations_NormalizedOrganizationName",
                        column: x => x.NormalizedOrganizationName,
                        principalTable: "Organizations",
                        principalColumn: "NormalizedName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NormalizedOrganizationName = table.Column<string>(maxLength: 128, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_Organizations_NormalizedOrganizationName",
                        column: x => x.NormalizedOrganizationName,
                        principalTable: "Organizations",
                        principalColumn: "NormalizedName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invitations_Users_NormalizedUserName",
                        column: x => x.NormalizedUserName,
                        principalTable: "Users",
                        principalColumn: "NormalizedUserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    sentDate = table.Column<DateTime>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_NormalizedUserName",
                        column: x => x.NormalizedUserName,
                        principalTable: "Users",
                        principalColumn: "NormalizedUserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NormalizedOrganizationName = table.Column<string>(maxLength: 128, nullable: false),
                    NormalizedName = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    ShortName = table.Column<string>(nullable: true),
                    LogoId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(type: "ntext", nullable: true),
                    ProjectType = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.UniqueConstraint("AK_Projects_NormalizedName_NormalizedOrganizationName", x => new { x.NormalizedName, x.NormalizedOrganizationName });
                    table.ForeignKey(
                        name: "FK_Projects_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Photos_LogoId",
                        column: x => x.LogoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Organizations_NormalizedOrganizationName",
                        column: x => x.NormalizedOrganizationName,
                        principalTable: "Organizations",
                        principalColumn: "NormalizedName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NormalizedOrganizationName = table.Column<string>(maxLength: 128, nullable: false),
                    NormalizedUserName = table.Column<string>(maxLength: 128, nullable: false),
                    NormalizedTeamName = table.Column<string>(maxLength: 128, nullable: true),
                    MemberType = table.Column<short>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Organizations_NormalizedOrganizationName",
                        column: x => x.NormalizedOrganizationName,
                        principalTable: "Organizations",
                        principalColumn: "NormalizedName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Users_NormalizedUserName",
                        column: x => x.NormalizedUserName,
                        principalTable: "Users",
                        principalColumn: "NormalizedUserName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Teams_NormalizedOrganizationName_NormalizedTeamName",
                        columns: x => new { x.NormalizedOrganizationName, x.NormalizedTeamName },
                        principalTable: "Teams",
                        principalColumns: new[] { "NormalizedOrganizationName", "NormalizedName" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccessList",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NormalizedOrganizationName = table.Column<string>(maxLength: 128, nullable: false),
                    NormalizedTeamName = table.Column<string>(maxLength: 128, nullable: false),
                    NormalizedProjectName = table.Column<string>(maxLength: 128, nullable: false),
                    Permission = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessList", x => x.Id);
                    table.UniqueConstraint("AK_AccessList_NormalizedOrganizationName_NormalizedProjectName_NormalizedTeamName", x => new { x.NormalizedOrganizationName, x.NormalizedProjectName, x.NormalizedTeamName });
                    table.ForeignKey(
                        name: "FK_AccessList_Projects_NormalizedProjectName_NormalizedOrganizationName",
                        columns: x => new { x.NormalizedProjectName, x.NormalizedOrganizationName },
                        principalTable: "Projects",
                        principalColumns: new[] { "NormalizedName", "NormalizedOrganizationName" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessList_Teams_NormalizedTeamName_NormalizedOrganizationName",
                        columns: x => new { x.NormalizedTeamName, x.NormalizedOrganizationName },
                        principalTable: "Teams",
                        principalColumns: new[] { "NormalizedName", "NormalizedOrganizationName" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NormalizedProjectName = table.Column<string>(maxLength: 128, nullable: false),
                    NormalizedOrganizationName = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    EarlyStart = table.Column<DateTime>(nullable: true),
                    EarlyFinish = table.Column<DateTime>(nullable: true),
                    LateStart = table.Column<DateTime>(nullable: true),
                    LateFinish = table.Column<DateTime>(nullable: true),
                    FreeFloat = table.Column<double>(nullable: false),
                    TotalFloat = table.Column<double>(nullable: false),
                    Duration = table.Column<double>(nullable: false),
                    Priority = table.Column<byte>(nullable: false),
                    EstimatedHours = table.Column<double>(nullable: false),
                    HoursSpent = table.Column<double>(nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.UniqueConstraint("AK_Activities_Name_NormalizedProjectName_NormalizedOrganizationName", x => new { x.Name, x.NormalizedProjectName, x.NormalizedOrganizationName });
                    table.ForeignKey(
                        name: "FK_Activities_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activities_Projects_NormalizedProjectName_NormalizedOrganizationName",
                        columns: x => new { x.NormalizedProjectName, x.NormalizedOrganizationName },
                        principalTable: "Projects",
                        principalColumns: new[] { "NormalizedName", "NormalizedOrganizationName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Arrows",
                columns: table => new
                {
                    NormalizedOrganizationName = table.Column<string>(maxLength: 128, nullable: false),
                    NormalizedProjectName = table.Column<string>(maxLength: 128, nullable: false),
                    FromActivityName = table.Column<string>(nullable: false),
                    ToActivityName = table.Column<string>(nullable: false),
                    ConstraintValue = table.Column<double>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arrows", x => new { x.NormalizedProjectName, x.NormalizedOrganizationName, x.FromActivityName, x.ToActivityName });
                    table.ForeignKey(
                        name: "FK_Arrows_Activities_FromActivityName_NormalizedProjectName_NormalizedOrganizationName",
                        columns: x => new { x.FromActivityName, x.NormalizedProjectName, x.NormalizedOrganizationName },
                        principalTable: "Activities",
                        principalColumns: new[] { "Name", "NormalizedProjectName", "NormalizedOrganizationName" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Arrows_Activities_ToActivityName_NormalizedProjectName_NormalizedOrganizationName",
                        columns: x => new { x.ToActivityName, x.NormalizedProjectName, x.NormalizedOrganizationName },
                        principalTable: "Activities",
                        principalColumns: new[] { "Name", "NormalizedProjectName", "NormalizedOrganizationName" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActivityName = table.Column<string>(maxLength: 128, nullable: false),
                    NormalizedTeamName = table.Column<string>(maxLength: 128, nullable: false),
                    NormalizedOrganizationName = table.Column<string>(maxLength: 128, nullable: false),
                    NormalizedProjectName = table.Column<string>(maxLength: 128, nullable: false),
                    MemberId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                    table.UniqueConstraint("AK_Resources_ActivityName_NormalizedTeamName_NormalizedOrganizationName_NormalizedProjectName", x => new { x.ActivityName, x.NormalizedTeamName, x.NormalizedOrganizationName, x.NormalizedProjectName });
                    table.ForeignKey(
                        name: "FK_Resources_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resources_Teams_NormalizedTeamName_NormalizedOrganizationName",
                        columns: x => new { x.NormalizedTeamName, x.NormalizedOrganizationName },
                        principalTable: "Teams",
                        principalColumns: new[] { "NormalizedName", "NormalizedOrganizationName" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resources_Activities_ActivityName_NormalizedProjectName_NormalizedOrganizationName",
                        columns: x => new { x.ActivityName, x.NormalizedProjectName, x.NormalizedOrganizationName },
                        principalTable: "Activities",
                        principalColumns: new[] { "Name", "NormalizedProjectName", "NormalizedOrganizationName" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessList_NormalizedProjectName_NormalizedOrganizationName",
                table: "AccessList",
                columns: new[] { "NormalizedProjectName", "NormalizedOrganizationName" });

            migrationBuilder.CreateIndex(
                name: "IX_AccessList_NormalizedTeamName_NormalizedOrganizationName",
                table: "AccessList",
                columns: new[] { "NormalizedTeamName", "NormalizedOrganizationName" });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_OrganizationId",
                table: "Activities",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_NormalizedProjectName_NormalizedOrganizationName",
                table: "Activities",
                columns: new[] { "NormalizedProjectName", "NormalizedOrganizationName" });

            migrationBuilder.CreateIndex(
                name: "IX_Arrows_FromActivityName_NormalizedProjectName_NormalizedOrganizationName",
                table: "Arrows",
                columns: new[] { "FromActivityName", "NormalizedProjectName", "NormalizedOrganizationName" });

            migrationBuilder.CreateIndex(
                name: "IX_Arrows_ToActivityName_NormalizedProjectName_NormalizedOrganizationName",
                table: "Arrows",
                columns: new[] { "ToActivityName", "NormalizedProjectName", "NormalizedOrganizationName" });

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_NormalizedOrganizationName",
                table: "Invitations",
                column: "NormalizedOrganizationName");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_NormalizedUserName",
                table: "Invitations",
                column: "NormalizedUserName");

            migrationBuilder.CreateIndex(
                name: "IX_Members_NormalizedUserName",
                table: "Members",
                column: "NormalizedUserName");

            migrationBuilder.CreateIndex(
                name: "IX_Members_NormalizedOrganizationName_NormalizedTeamName",
                table: "Members",
                columns: new[] { "NormalizedOrganizationName", "NormalizedTeamName" });

            migrationBuilder.CreateIndex(
                name: "IX_Members_NormalizedOrganizationName_NormalizedUserName_NormalizedTeamName",
                table: "Members",
                columns: new[] { "NormalizedOrganizationName", "NormalizedUserName", "NormalizedTeamName" },
                unique: true,
                filter: "[NormalizedOrganizationName] IS NOT NULL AND [NormalizedUserName] IS NOT NULL AND [NormalizedTeamName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NormalizedUserName",
                table: "Notifications",
                column: "NormalizedUserName");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_LogoId",
                table: "Organizations",
                column: "LogoId",
                unique: true,
                filter: "[LogoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_NormalizedName",
                table: "Organizations",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ClientId",
                table: "Projects",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_LogoId",
                table: "Projects",
                column: "LogoId",
                unique: true,
                filter: "[LogoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_NormalizedOrganizationName",
                table: "Projects",
                column: "NormalizedOrganizationName");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_MemberId",
                table: "Resources",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_NormalizedTeamName_NormalizedOrganizationName",
                table: "Resources",
                columns: new[] { "NormalizedTeamName", "NormalizedOrganizationName" });

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ActivityName_NormalizedProjectName_NormalizedOrganizationName",
                table: "Resources",
                columns: new[] { "ActivityName", "NormalizedProjectName", "NormalizedOrganizationName" });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhotoId",
                table: "Users",
                column: "PhotoId",
                unique: true,
                filter: "[PhotoId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessList");

            migrationBuilder.DropTable(
                name: "Arrows");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Photos");
        }
    }
}
