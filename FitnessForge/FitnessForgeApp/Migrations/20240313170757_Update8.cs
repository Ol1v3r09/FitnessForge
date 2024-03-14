using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessForgeApp.Migrations
{
    /// <inheritdoc />
    public partial class Update8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Fiber",
                table: "product",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Fiber",
                table: "product",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");
        }
    }
}
