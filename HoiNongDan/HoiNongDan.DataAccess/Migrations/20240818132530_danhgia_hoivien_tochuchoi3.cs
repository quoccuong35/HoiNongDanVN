using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class danhgia_hoivien_tochuchoi3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 8, 18, 20, 25, 29, 137, DateTimeKind.Local).AddTicks(460),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 8, 18, 20, 23, 9, 40, DateTimeKind.Local).AddTicks(6235));

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedAccountId",
                schema: "HV",
                table: "DanhGiaToChucHoi",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedTime",
                schema: "HV",
                table: "DanhGiaToChucHoi",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedAccountId",
                schema: "HV",
                table: "DanhGiaHoiVien",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedTime",
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
                defaultValue: new DateTime(2024, 8, 18, 20, 25, 29, 136, DateTimeKind.Local).AddTicks(7795),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 8, 18, 20, 23, 9, 40, DateTimeKind.Local).AddTicks(3507));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifiedAccountId",
                schema: "HV",
                table: "DanhGiaToChucHoi");

            migrationBuilder.DropColumn(
                name: "LastModifiedTime",
                schema: "HV",
                table: "DanhGiaToChucHoi");

            migrationBuilder.DropColumn(
                name: "LastModifiedAccountId",
                schema: "HV",
                table: "DanhGiaHoiVien");

            migrationBuilder.DropColumn(
                name: "LastModifiedTime",
                schema: "HV",
                table: "DanhGiaHoiVien");

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
                oldDefaultValue: new DateTime(2024, 8, 18, 20, 25, 29, 137, DateTimeKind.Local).AddTicks(460));

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
                oldDefaultValue: new DateTime(2024, 8, 18, 20, 25, 29, 136, DateTimeKind.Local).AddTicks(7795));
        }
    }
}
