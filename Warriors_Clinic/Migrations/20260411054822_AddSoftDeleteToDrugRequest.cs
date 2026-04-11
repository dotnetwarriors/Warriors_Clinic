using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warriors_Clinic.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToDrugRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeletedByPhysician",
                table: "DrugRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeletedByPhysician",
                table: "DrugRequests");
        }
    }
}
