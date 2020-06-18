using Microsoft.EntityFrameworkCore.Migrations;

namespace Intranet.DataAccess.Data.Migrations.CorpComm
{
    public partial class RemoveCritCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CritCount",
                table: "Collaterals");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CritCount",
                table: "Collaterals",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
