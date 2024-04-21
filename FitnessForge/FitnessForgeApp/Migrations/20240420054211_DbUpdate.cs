using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessForgeApp.Migrations
{
    /// <inheritdoc />
    public partial class DbUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalorieIntake",
                table: "daily_intake");

            migrationBuilder.DropColumn(
                name: "FluidIntake",
                table: "daily_intake");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CalorieIntake",
                table: "daily_intake",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FluidIntake",
                table: "daily_intake",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
