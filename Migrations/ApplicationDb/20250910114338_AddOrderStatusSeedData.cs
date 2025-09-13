using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class AddOrderStatusSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "CartDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "CartDetails");
        }
    }
}
