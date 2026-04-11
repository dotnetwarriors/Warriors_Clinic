using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warriors_Clinic.Migrations
{
    public partial class AddDosageFieldsOnly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Dosage",
                table: "PhysicianPrescription",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Timing",
                table: "PhysicianPrescription",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "PhysicianPrescription",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dosage",
                table: "PhysicianPrescription");

            migrationBuilder.DropColumn(
                name: "Timing",
                table: "PhysicianPrescription");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "PhysicianPrescription");
        }
    }
}