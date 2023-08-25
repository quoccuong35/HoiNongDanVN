using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class HBHoiDapEdit2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                defaultValue: new DateTime(2023, 8, 24, 7, 48, 19, 727, DateTimeKind.Local).AddTicks(9079),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 24, 7, 45, 14, 656, DateTimeKind.Local).AddTicks(8));

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
                name: "NoiDungTraLoi",
                schema: "HV",
                table: "HoiVienHoiDap",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrangThaiTraLoi",
                schema: "HV",
                table: "HoiVienHoiDap",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 24, 7, 48, 19, 727, DateTimeKind.Local).AddTicks(5030),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 24, 7, 45, 14, 655, DateTimeKind.Local).AddTicks(7273));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayTraLoi",
                schema: "HV",
                table: "HoiVienHoiDap");

            migrationBuilder.DropColumn(
                name: "NguoiTraLoi",
                schema: "HV",
                table: "HoiVienHoiDap");

            migrationBuilder.DropColumn(
                name: "NoiDungTraLoi",
                schema: "HV",
                table: "HoiVienHoiDap");

            migrationBuilder.DropColumn(
                name: "TrangThaiTraLoi",
                schema: "HV",
                table: "HoiVienHoiDap");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 24, 7, 45, 14, 656, DateTimeKind.Local).AddTicks(8),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 24, 7, 48, 19, 727, DateTimeKind.Local).AddTicks(9079));

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
                defaultValue: new DateTime(2023, 8, 24, 7, 45, 14, 655, DateTimeKind.Local).AddTicks(7273),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 24, 7, 48, 19, 727, DateTimeKind.Local).AddTicks(5030));
        }
    }
}
