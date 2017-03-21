using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace asp.netcoretripmanager.Migrations
{
    public partial class AddForeignKeyToStops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stops_Trips_TripId",
                table: "Stops");

            migrationBuilder.AlterColumn<int>(
                name: "TripId",
                table: "Stops",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Trips_TripId",
                table: "Stops",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stops_Trips_TripId",
                table: "Stops");

            migrationBuilder.AlterColumn<int>(
                name: "TripId",
                table: "Stops",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Trips_TripId",
                table: "Stops",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
