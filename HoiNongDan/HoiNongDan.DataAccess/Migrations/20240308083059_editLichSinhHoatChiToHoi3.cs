using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editLichSinhHoatChiToHoi3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 8, 15, 30, 57, 999, DateTimeKind.Local).AddTicks(2191),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 7, 14, 53, 4, 67, DateTimeKind.Local).AddTicks(9539));

            migrationBuilder.AlterColumn<string>(
                name: "NoiDungSinhHoat",
                schema: "HV",
                table: "LichSinhHoatChiToHoi",
                type: "nvarchar(max)",
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
                defaultValue: new DateTime(2024, 3, 8, 15, 30, 57, 998, DateTimeKind.Local).AddTicks(9672),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 7, 14, 53, 4, 67, DateTimeKind.Local).AddTicks(5347));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 7, 14, 53, 4, 67, DateTimeKind.Local).AddTicks(9539),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 8, 15, 30, 57, 999, DateTimeKind.Local).AddTicks(2191));

            migrationBuilder.AlterColumn<string>(
                name: "NoiDungSinhHoat",
                schema: "HV",
                table: "LichSinhHoatChiToHoi",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 7, 14, 53, 4, 67, DateTimeKind.Local).AddTicks(5347),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 8, 15, 30, 57, 998, DateTimeKind.Local).AddTicks(9672));
        }
    }
}
