using Microsoft.EntityFrameworkCore.Migrations;

namespace Intranet.DataAccess.Data.Migrations.CorpComm
{
    public partial class AddCollateralToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Collaterals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(nullable: false),
                    SizeId = table.Column<int>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    BrandId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    ImgUrl = table.Column<string>(nullable: false),
                    isActive = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    UserIP = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    UserDate = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaterals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collaterals_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Collaterals_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Collaterals_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Collaterals_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collaterals_BrandId",
                table: "Collaterals",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Collaterals_LocationId",
                table: "Collaterals",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Collaterals_SizeId",
                table: "Collaterals",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Collaterals_UnitId",
                table: "Collaterals",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Collaterals");
        }
    }
}
