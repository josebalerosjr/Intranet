using Microsoft.EntityFrameworkCore.Migrations;

namespace Intranet.DataAccess.Data.Migrations.CorpComm
{
    public partial class RemoveStatusAndLocationIdFromDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_Locations_LocationId",
                table: "OrderHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_Statuses_StatusId",
                table: "OrderHeaders");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeaders_LocationId",
                table: "OrderHeaders");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeaders_StatusId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "OrderHeaders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "OrderHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "OrderHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_LocationId",
                table: "OrderHeaders",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_StatusId",
                table: "OrderHeaders",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_Locations_LocationId",
                table: "OrderHeaders",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_Statuses_StatusId",
                table: "OrderHeaders",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
