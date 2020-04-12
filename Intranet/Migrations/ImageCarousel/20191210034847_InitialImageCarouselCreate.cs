using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Intranet.Migrations.ImageCarousel
{
    public partial class InitialImageCarouselCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "imageCarousels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImageName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ImageLink = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    UserIP = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    UserDate = table.Column<string>(type: "nvarchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_imageCarousels", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "imageCarousels");
        }
    }
}