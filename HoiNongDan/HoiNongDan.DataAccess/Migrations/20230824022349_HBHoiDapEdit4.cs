using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class HBHoiDapEdit4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                defaultValue: new DateTime(2023, 8, 24, 9, 23, 47, 519, DateTimeKind.Local).AddTicks(9347),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 24, 8, 57, 24, 366, DateTimeKind.Local).AddTicks(2747));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 24, 9, 23, 47, 519, DateTimeKind.Local).AddTicks(6521),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 24, 8, 57, 24, 366, DateTimeKind.Local).AddTicks(150));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 24, 8, 57, 24, 366, DateTimeKind.Local).AddTicks(2747),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 24, 9, 23, 47, 519, DateTimeKind.Local).AddTicks(9347));

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
                type: "nvarchar(2000)",
                maxLength: 2000,
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
                defaultValue: new DateTime(2023, 8, 24, 8, 57, 24, 366, DateTimeKind.Local).AddTicks(150),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 24, 9, 23, 47, 519, DateTimeKind.Local).AddTicks(6521));
        }
    }
}
