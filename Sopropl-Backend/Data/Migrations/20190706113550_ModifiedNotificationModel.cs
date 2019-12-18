using Microsoft.EntityFrameworkCore.Migrations;

namespace Sopropl_Backend.Data.Migrations
{
    public partial class ModifiedNotificationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MemberType",
                table: "Members",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Type",
                table: "Notifications",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Members",
                newName: "MemberType");
        }
    }
}
