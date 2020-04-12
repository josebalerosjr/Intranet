using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Intranet.Migrations.BdoPE
{
    public partial class InitialBdoPECreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bdoPEs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DocDateInDoc = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    DocType = table.Column<string>(type: "nvarchar(2)", nullable: true),
                    CompanyCode = table.Column<string>(type: "nvarchar(4)", nullable: true),
                    PosDateInDoc = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    FiscalPeriod = table.Column<string>(type: "nvarchar(2)", nullable: true),
                    CurrentKey = table.Column<string>(type: "nvarchar(3)", nullable: true),
                    RefDocNum = table.Column<string>(type: "nvarchar(32)", nullable: true),
                    DocHeadT = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    PosKeyInNextLine = table.Column<string>(type: "nvarchar(2)", nullable: true),
                    AccMatNextLine = table.Column<string>(type: "nvarchar(8)", nullable: true),
                    AmountDocCur = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    ValDate = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    AssignNum = table.Column<string>(type: "nvarchar(32)", nullable: true),
                    ItemText = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    PosKeyInNextLine2 = table.Column<string>(type: "nvarchar(2)", nullable: true),
                    AccMatNextLine2 = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    AmountDocCur2 = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    BaseDateDueCal = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    ItemText2 = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    MarketerZ2 = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    isDownloaded = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    UserIP = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    UserDate = table.Column<string>(type: "nvarchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bdoPEs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bdoPEs");
        }
    }
}