using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Intranet.DataAccess.Data.Migrations.CorpComm
{
    public partial class AddHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(nullable: true),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    LoginUser = table.Column<string>(nullable: true),
                    CollateralName = table.Column<string>(nullable: true),
                    EventType = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    StationEvent = table.Column<string>(nullable: true),
                    EventDate = table.Column<DateTime>(nullable: false),
                    ShippingDate = table.Column<DateTime>(nullable: false),
                    DropOffPoint = table.Column<string>(nullable: true),
                    rating = table.Column<int>(nullable: false),
                    RejectMarks = table.Column<string>(nullable: true),
                    ReconRemarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Histories");
        }
    }
}