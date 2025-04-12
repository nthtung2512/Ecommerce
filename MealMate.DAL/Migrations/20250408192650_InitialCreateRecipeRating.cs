using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateRecipeRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AverageRating",
                table: "recipe",
                type: "numeric(2,1)",
                precision: 2,
                scale: 1,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "recipe");
        }
    }
}
