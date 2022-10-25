using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeninsulaPhysiotherapy.Migrations
{
    public partial class addther : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TherapistVM",
                table: "TherapistVM");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "TherapistVM",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "TherapistVM",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TherapistVM",
                table: "TherapistVM",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TherapistVM",
                table: "TherapistVM");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TherapistVM");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "TherapistVM",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TherapistVM",
                table: "TherapistVM",
                column: "FullName");
        }
    }
}
