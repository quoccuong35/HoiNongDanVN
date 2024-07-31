using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editHoTro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DenNgay",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropColumn(
                name: "LaiSuatVay",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropColumn(
                name: "NgayTraNoCuoiCung",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropColumn(
                name: "SoTienVay",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropColumn(
                name: "ThoiHangChoVay",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropColumn(
                name: "TraXong",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropColumn(
                name: "TuNgay",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 4, 8, 20, 0, 3, 882, DateTimeKind.Local).AddTicks(8106),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 4, 6, 20, 15, 51, 348, DateTimeKind.Local).AddTicks(377));

            migrationBuilder.AddColumn<Guid>(
                name: "IDLopHoc",
                schema: "HV",
                table: "HoiVienHoTro",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 4, 8, 20, 0, 3, 882, DateTimeKind.Local).AddTicks(5774),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 4, 6, 20, 15, 51, 347, DateTimeKind.Local).AddTicks(8185));

            migrationBuilder.CreateIndex(
                name: "IX_HoiVienHoTro_IDLopHoc",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "IDLopHoc");

            migrationBuilder.AddForeignKey(
                name: "FK_HoiVienVayVon_LopHoc",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "IDLopHoc",
                principalSchema: "tMasterData",
                principalTable: "LopHoc",
                principalColumn: "IDLopHoc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoiVienVayVon_LopHoc",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropIndex(
                name: "IX_HoiVienHoTro_IDLopHoc",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropColumn(
                name: "IDLopHoc",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 4, 6, 20, 15, 51, 348, DateTimeKind.Local).AddTicks(377),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 4, 8, 20, 0, 3, 882, DateTimeKind.Local).AddTicks(8106));

            migrationBuilder.AddColumn<DateTime>(
                name: "DenNgay",
                schema: "HV",
                table: "HoiVienHoTro",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LaiSuatVay",
                schema: "HV",
                table: "HoiVienHoTro",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTraNoCuoiCung",
                schema: "HV",
                table: "HoiVienHoTro",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SoTienVay",
                schema: "HV",
                table: "HoiVienHoTro",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThoiHangChoVay",
                schema: "HV",
                table: "HoiVienHoTro",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TraXong",
                schema: "HV",
                table: "HoiVienHoTro",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TuNgay",
                schema: "HV",
                table: "HoiVienHoTro",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 4, 6, 20, 15, 51, 347, DateTimeKind.Local).AddTicks(8185),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 4, 8, 20, 0, 3, 882, DateTimeKind.Local).AddTicks(5774));
        }
    }
}
