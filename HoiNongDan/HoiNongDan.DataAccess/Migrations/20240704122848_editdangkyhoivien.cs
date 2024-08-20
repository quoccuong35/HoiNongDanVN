using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editdangkyhoivien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 4, 19, 28, 46, 508, DateTimeKind.Local).AddTicks(3420),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 3, 19, 58, 10, 273, DateTimeKind.Local).AddTicks(9065));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 4, 19, 28, 46, 508, DateTimeKind.Local).AddTicks(225),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 3, 19, 58, 10, 273, DateTimeKind.Local).AddTicks(4313));

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountIdDangKy",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "NgayDangKy",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 3, 19, 58, 10, 273, DateTimeKind.Local).AddTicks(9065),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 4, 19, 28, 46, 508, DateTimeKind.Local).AddTicks(3420));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 3, 19, 58, 10, 273, DateTimeKind.Local).AddTicks(4313),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 4, 19, 28, 46, 508, DateTimeKind.Local).AddTicks(225));
        }
    }
}
