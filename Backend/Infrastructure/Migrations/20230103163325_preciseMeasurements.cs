using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class preciseMeasurements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Minimum",
                table: "Measurements",
                newName: "MinimumSpO2");

            migrationBuilder.RenameColumn(
                name: "Maximum",
                table: "Measurements",
                newName: "MinimumHeartRate");

            migrationBuilder.RenameColumn(
                name: "Average",
                table: "Measurements",
                newName: "MaximumSpO2");

            migrationBuilder.AddColumn<int>(
                name: "AverageHeartRate",
                table: "Measurements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AverageSpO2",
                table: "Measurements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaximumHeartRate",
                table: "Measurements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageHeartRate",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "AverageSpO2",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "MaximumHeartRate",
                table: "Measurements");

            migrationBuilder.RenameColumn(
                name: "MinimumSpO2",
                table: "Measurements",
                newName: "Minimum");

            migrationBuilder.RenameColumn(
                name: "MinimumHeartRate",
                table: "Measurements",
                newName: "Maximum");

            migrationBuilder.RenameColumn(
                name: "MaximumSpO2",
                table: "Measurements",
                newName: "Average");
        }
    }
}
