using Microsoft.EntityFrameworkCore.Migrations;

namespace Intranet.DataAccess.Data.Migrations.CorpComm
{
    public partial class AddStationToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StationTypeId = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    UserIP = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    UserDate = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stations_StationTypes_StationTypeId",
                        column: x => x.StationTypeId,
                        principalTable: "StationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stations_StationTypeId",
                table: "Stations",
                column: "StationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stations");
        }
    }
}
