using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editDoanTheChinhTri_HoiDoan_HoiVien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifiedAccountId",
                schema: "HV",
                table: "DoanTheChinhTri_HoiDoan_HoiVien");

            migrationBuilder.DropColumn(
                name: "LastModifiedTime",
                schema: "HV",
                table: "DoanTheChinhTri_HoiDoan_HoiVien");

            migrationBuilder.DropColumn(
                name: "LyDoRoi",
                schema: "HV",
                table: "DoanTheChinhTri_HoiDoan_HoiVien");

            migrationBuilder.DropColumn(
                name: "NgayRoi",
                schema: "HV",
                table: "DoanTheChinhTri_HoiDoan_HoiVien");

            migrationBuilder.DropColumn(
                name: "NgayThamGia",
                schema: "HV",
                table: "DoanTheChinhTri_HoiDoan_HoiVien");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 7, 7, 46, 59, 816, DateTimeKind.Local).AddTicks(3331),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 4, 15, 11, 39, 210, DateTimeKind.Local).AddTicks(7728));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 7, 7, 46, 59, 816, DateTimeKind.Local).AddTicks(722),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 4, 15, 11, 39, 210, DateTimeKind.Local).AddTicks(5251));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 4, 15, 11, 39, 210, DateTimeKind.Local).AddTicks(7728),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 7, 7, 46, 59, 816, DateTimeKind.Local).AddTicks(3331));

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedAccountId",
                schema: "HV",
                table: "DoanTheChinhTri_HoiDoan_HoiVien",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedTime",
                schema: "HV",
                table: "DoanTheChinhTri_HoiDoan_HoiVien",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LyDoRoi",
                schema: "HV",
                table: "DoanTheChinhTri_HoiDoan_HoiVien",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayRoi",
                schema: "HV",
                table: "DoanTheChinhTri_HoiDoan_HoiVien",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayThamGia",
                schema: "HV",
                table: "DoanTheChinhTri_HoiDoan_HoiVien",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 4, 15, 11, 39, 210, DateTimeKind.Local).AddTicks(5251),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 7, 7, 46, 59, 816, DateTimeKind.Local).AddTicks(722));
        }
    }
}
