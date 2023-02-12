using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class wifiIdNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_Wifis_WifiId",
                table: "Measurements");

            migrationBuilder.AlterColumn<Guid>(
                name: "WifiId",
                table: "Measurements",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_Wifis_WifiId",
                table: "Measurements",
                column: "WifiId",
                principalTable: "Wifis",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_Wifis_WifiId",
                table: "Measurements");

            migrationBuilder.AlterColumn<Guid>(
                name: "WifiId",
                table: "Measurements",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_Wifis_WifiId",
                table: "Measurements",
                column: "WifiId",
                principalTable: "Wifis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
