using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class danhgia_hoivien_tochuchoi1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 8, 18, 17, 55, 22, 341, DateTimeKind.Local).AddTicks(5537),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 8, 18, 17, 49, 9, 290, DateTimeKind.Local).AddTicks(6017));

            migrationBuilder.AlterColumn<string>(
                name: "LoaiToChuc",
                schema: "HV",
                table: "DanhGiaToChucHoi",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LoaiDanhGia",
                schema: "HV",
                table: "DanhGiaToChucHoi",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                schema: "HV",
                table: "DanhGiaToChucHoi",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LoaiDanhGia",
                schema: "HV",
                table: "DanhGiaHoiVien",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                schema: "HV",
                table: "DanhGiaHoiVien",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 8, 18, 17, 55, 22, 341, DateTimeKind.Local).AddTicks(2975),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 8, 18, 17, 49, 9, 290, DateTimeKind.Local).AddTicks(4053));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 8, 18, 17, 49, 9, 290, DateTimeKind.Local).AddTicks(6017),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 8, 18, 17, 55, 22, 341, DateTimeKind.Local).AddTicks(5537));

            migrationBuilder.AlterColumn<string>(
                name: "LoaiToChuc",
                schema: "HV",
                table: "DanhGiaToChucHoi",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "LoaiDanhGia",
                schema: "HV",
                table: "DanhGiaToChucHoi",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                schema: "HV",
                table: "DanhGiaToChucHoi",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoaiDanhGia",
                schema: "HV",
                table: "DanhGiaHoiVien",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "GhiChu",
                schema: "HV",
                table: "DanhGiaHoiVien",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 8, 18, 17, 49, 9, 290, DateTimeKind.Local).AddTicks(4053),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 8, 18, 17, 55, 22, 341, DateTimeKind.Local).AddTicks(2975));
        }
    }
}
