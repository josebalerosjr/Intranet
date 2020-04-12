using Microsoft.EntityFrameworkCore.Migrations;

namespace Intranet.Migrations.ImageCarousel
{
    public partial class AddCarouselActiveColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "imageCarousels",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "imageCarousels");
        }
    }
}
