using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editQuaTrinhCongTac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBanChapHanh",
                schema: "NS",
                table: "QuaTrinhCongTac",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NhiemKy",
                schema: "NS",
                table: "QuaTrinhCongTac",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 18, 14, 5, 51, 549, DateTimeKind.Local).AddTicks(228),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 10, 19, 25, 37, 520, DateTimeKind.Local).AddTicks(8536));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 18, 14, 5, 51, 548, DateTimeKind.Local).AddTicks(7855),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 10, 19, 25, 37, 520, DateTimeKind.Local).AddTicks(3601));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBanChapHanh",
                schema: "NS",
                table: "QuaTrinhCongTac");

            migrationBuilder.DropColumn(
                name: "NhiemKy",
                schema: "NS",
                table: "QuaTrinhCongTac");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 10, 19, 25, 37, 520, DateTimeKind.Local).AddTicks(8536),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 18, 14, 5, 51, 549, DateTimeKind.Local).AddTicks(228));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 10, 19, 25, 37, 520, DateTimeKind.Local).AddTicks(3601),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 18, 14, 5, 51, 548, DateTimeKind.Local).AddTicks(7855));
        }
    }
}
