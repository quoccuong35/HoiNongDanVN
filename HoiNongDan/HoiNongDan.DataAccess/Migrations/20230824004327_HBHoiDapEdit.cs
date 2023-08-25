using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class HBHoiDapEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 24, 7, 43, 26, 429, DateTimeKind.Local).AddTicks(4216),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 22, 21, 51, 46, 170, DateTimeKind.Local).AddTicks(8822));

            migrationBuilder.AddColumn<DateTime>(
                name: "Ngay",
                schema: "HV",
                table: "HoiVienHoiDap",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTraLoi",
                schema: "HV",
                table: "HoiVienHoiDap",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NguoiTraLoi",
                schema: "HV",
                table: "HoiVienHoiDap",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TieuDe",
                schema: "HV",
                table: "HoiVienHoiDap",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 24, 7, 43, 26, 429, DateTimeKind.Local).AddTicks(1454),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 22, 21, 51, 46, 170, DateTimeKind.Local).AddTicks(5884));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ngay",
                schema: "HV",
                table: "HoiVienHoiDap");

            migrationBuilder.DropColumn(
                name: "NgayTraLoi",
                schema: "HV",
                table: "HoiVienHoiDap");

            migrationBuilder.DropColumn(
                name: "NguoiTraLoi",
                schema: "HV",
                table: "HoiVienHoiDap");

            migrationBuilder.DropColumn(
                name: "TieuDe",
                schema: "HV",
                table: "HoiVienHoiDap");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 22, 21, 51, 46, 170, DateTimeKind.Local).AddTicks(8822),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 24, 7, 43, 26, 429, DateTimeKind.Local).AddTicks(4216));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 22, 21, 51, 46, 170, DateTimeKind.Local).AddTicks(5884),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 24, 7, 43, 26, 429, DateTimeKind.Local).AddTicks(1454));
        }
    }
}
