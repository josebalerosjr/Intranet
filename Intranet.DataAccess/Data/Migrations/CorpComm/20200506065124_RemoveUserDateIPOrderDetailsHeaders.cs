using Microsoft.EntityFrameworkCore.Migrations;

namespace Intranet.DataAccess.Data.Migrations.CorpComm
{
    public partial class RemoveUserDateIPOrderDetailsHeaders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserDate",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "UserIP",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "UserDate",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "UserIP",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "OrderDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserDate",
                table: "OrderHeaders",
                type: "nvarchar(10)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserIP",
                table: "OrderHeaders",
                type: "nvarchar(15)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "OrderHeaders",
                type: "nvarchar(32)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserDate",
                table: "OrderDetails",
                type: "nvarchar(10)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserIP",
                table: "OrderDetails",
                type: "nvarchar(15)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "OrderDetails",
                type: "nvarchar(32)",
                nullable: false,
                defaultValue: "");
        }
    }
}
