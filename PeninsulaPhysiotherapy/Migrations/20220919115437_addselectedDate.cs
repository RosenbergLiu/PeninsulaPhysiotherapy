using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeninsulaPhysiotherapy.Migrations
{
    public partial class addselectedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelectedDate",
                table: "AppointmentVM",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedDate",
                table: "AppointmentVM");
        }
    }
}
