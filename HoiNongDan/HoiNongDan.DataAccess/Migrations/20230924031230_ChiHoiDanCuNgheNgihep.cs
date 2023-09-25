using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class ChiHoiDanCuNgheNgihep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 24, 10, 12, 29, 671, DateTimeKind.Local).AddTicks(5203),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 24, 9, 52, 8, 622, DateTimeKind.Local).AddTicks(1312));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 24, 10, 12, 29, 671, DateTimeKind.Local).AddTicks(2811),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 24, 9, 52, 8, 621, DateTimeKind.Local).AddTicks(8300));

            migrationBuilder.AddColumn<string>(
                name: "ChiHoiDanCu_CHP",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChiHoiDanCu_CHT",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChiHoiNgheNghiep_CHP",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChiHoiNgheNghiep_CHT",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChiHoiDanCu_CHP",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "ChiHoiDanCu_CHT",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "ChiHoiNgheNghiep_CHP",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "ChiHoiNgheNghiep_CHT",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 24, 9, 52, 8, 622, DateTimeKind.Local).AddTicks(1312),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 24, 10, 12, 29, 671, DateTimeKind.Local).AddTicks(5203));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 24, 9, 52, 8, 621, DateTimeKind.Local).AddTicks(8300),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 24, 10, 12, 29, 671, DateTimeKind.Local).AddTicks(2811));
        }
    }
}
