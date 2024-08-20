using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class danhgia_hoivien_tochuchoi2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 8, 18, 20, 23, 9, 40, DateTimeKind.Local).AddTicks(6235),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 8, 18, 17, 55, 22, 341, DateTimeKind.Local).AddTicks(5537));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedAccountId",
                schema: "HV",
                table: "DanhGiaToChucHoi",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                schema: "HV",
                table: "DanhGiaToChucHoi",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoLuong",
                schema: "HV",
                table: "DanhGiaToChucHoi",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedAccountId",
                schema: "HV",
                table: "DanhGiaHoiVien",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                schema: "HV",
                table: "DanhGiaHoiVien",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 8, 18, 20, 23, 9, 40, DateTimeKind.Local).AddTicks(3507),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 8, 18, 17, 55, 22, 341, DateTimeKind.Local).AddTicks(2975));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAccountId",
                schema: "HV",
                table: "DanhGiaToChucHoi");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                schema: "HV",
                table: "DanhGiaToChucHoi");

            migrationBuilder.DropColumn(
                name: "SoLuong",
                schema: "HV",
                table: "DanhGiaToChucHoi");

            migrationBuilder.DropColumn(
                name: "CreatedAccountId",
                schema: "HV",
                table: "DanhGiaHoiVien");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                schema: "HV",
                table: "DanhGiaHoiVien");

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
                oldDefaultValue: new DateTime(2024, 8, 18, 20, 23, 9, 40, DateTimeKind.Local).AddTicks(6235));

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
                oldDefaultValue: new DateTime(2024, 8, 18, 20, 23, 9, 40, DateTimeKind.Local).AddTicks(3507));
        }
    }
}
