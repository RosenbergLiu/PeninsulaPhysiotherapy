using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PakenhamPhysiotherapy.Data.Migrations
{
    public partial class DeleteCreatedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Job",
                newName: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Job",
                newName: "CreatedBy");
        }
    }
}
