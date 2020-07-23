using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Intranet.DataAccess.Data.Migrations.CorpComm
{
    public partial class AddEventTypeStationEventDateToOrderHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EventDate",
                table: "OrderHeaders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EventName",
                table: "OrderHeaders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StationEvent",
                table: "OrderHeaders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventDate",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "EventName",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "StationEvent",
                table: "OrderHeaders");
        }
    }
}
