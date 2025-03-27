using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateMMv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalWeight",
                table: "bill");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "product",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "PName",
                table: "product",
                newName: "Unit");

            migrationBuilder.RenameColumn(
                name: "ImageURL",
                table: "product",
                newName: "OriginalName");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "product",
                newName: "NameClean");

            migrationBuilder.AddColumn<string>(
                name: "Aisle",
                table: "product",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Consistency",
                table: "product",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "product",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "product",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aisle",
                table: "product");

            migrationBuilder.DropColumn(
                name: "Consistency",
                table: "product");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "product");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "product");

            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "product",
                newName: "PName");

            migrationBuilder.RenameColumn(
                name: "OriginalName",
                table: "product",
                newName: "ImageURL");

            migrationBuilder.RenameColumn(
                name: "NameClean",
                table: "product",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "product",
                newName: "Weight");

            migrationBuilder.AddColumn<int>(
                name: "TotalWeight",
                table: "bill",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
