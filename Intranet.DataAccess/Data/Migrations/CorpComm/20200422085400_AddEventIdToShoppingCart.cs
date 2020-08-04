using Microsoft.EntityFrameworkCore.Migrations;

namespace Intranet.DataAccess.Data.Migrations.CorpComm
{
    public partial class AddEventIdToShoppingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "ShoppingCarts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_EventId",
                table: "ShoppingCarts",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Events_EventId",
                table: "ShoppingCarts",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Events_EventId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_EventId",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "ShoppingCarts");
        }
    }
}