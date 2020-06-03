using Microsoft.EntityFrameworkCore.Migrations;

namespace Intranet.DataAccess.Data.Migrations.CorpComm
{
    public partial class AddPickUpPointAndAddReqEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PickUpPoints",
                table: "OrderHeaders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestorEmail",
                table: "OrderHeaders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PickUpPoints",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "RequestorEmail",
                table: "OrderHeaders");
        }
    }
}
