using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalClinic.ManagementSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Phase1FinalHardening : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Appointments_Status",
                table: "Appointments",
                sql: "[Status] IN ('Scheduled', 'Completed', 'Cancelled', 'NoShow')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Appointments_Status",
                table: "Appointments");
        }
    }
}
