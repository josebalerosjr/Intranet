using Microsoft.EntityFrameworkCore.Migrations;

namespace Intranet.DataAccess.Data.Migrations.CorpComm
{
    public partial class ChangeStringToIntRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderRating",
                table: "OrderHeaders",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OrderRating",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
