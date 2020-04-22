using Microsoft.EntityFrameworkCore.Migrations;

namespace Intranet.DataAccess.Data.Migrations.CorpComm
{
    public partial class RemoveRequiredOnImgUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImgUrl",
                table: "Collaterals",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImgUrl",
                table: "Collaterals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
