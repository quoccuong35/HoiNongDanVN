using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class HBHoiDapEdit3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                oldDefaultValue: new DateTime(2023, 8, 24, 7, 48, 19, 727, DateTimeKind.Local).AddTicks(9079));

            migrationBuilder.AlterColumn<string>(
                name: "NoiDungTraLoi",
                schema: "HV",
                table: "HoiVienHoiDap",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NoiDung",
                schema: "HV",
                table: "HoiVienHoiDap",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

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
                oldDefaultValue: new DateTime(2023, 8, 24, 7, 48, 19, 727, DateTimeKind.Local).AddTicks(5030));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                oldDefaultValue: new DateTime(2023, 8, 24, 8, 57, 24, 366, DateTimeKind.Local).AddTicks(2747));

            migrationBuilder.AlterColumn<string>(
                name: "NoiDungTraLoi",
                schema: "HV",
                table: "HoiVienHoiDap",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NoiDung",
                schema: "HV",
                table: "HoiVienHoiDap",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

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
                oldDefaultValue: new DateTime(2023, 8, 24, 8, 57, 24, 366, DateTimeKind.Local).AddTicks(150));
        }
    }
}
