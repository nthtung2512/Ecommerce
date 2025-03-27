using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateMMv21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleCapacity",
                table: "shipper");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VehicleCapacity",
                table: "shipper",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
