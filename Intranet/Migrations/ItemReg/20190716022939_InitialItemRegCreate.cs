using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Intranet.Migrations.ItemReg
{
    public partial class InitialItemRegCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemRegs",
                columns: table => new
                {
                    ItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ItemName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ItemDesc = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    ManufName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    AsstSerial = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    PartNum = table.Column<string>(type: "nvarchar(32)", nullable: true),
                    TypeName = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    CalDate = table.Column<DateTime>(nullable: true),
                    Qty = table.Column<int>(nullable: false),
                    CritLevel = table.Column<int>(nullable: false),
                    UnitName = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    LocName = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(32)", nullable: false),
                    UserIP = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    UserDate = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemRegs", x => x.ItemId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemRegs");
        }
    }
}