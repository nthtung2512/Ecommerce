using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "store",
                type: "numeric(10,7)",
                precision: 10,
                scale: 7,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "store",
                type: "numeric(10,7)",
                precision: 10,
                scale: 7,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress",
                table: "bill",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "store");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "store");

            migrationBuilder.DropColumn(
                name: "ShippingAddress",
                table: "bill");
        }
    }
}
