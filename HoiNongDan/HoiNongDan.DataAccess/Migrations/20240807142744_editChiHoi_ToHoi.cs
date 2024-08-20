using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editChiHoi_ToHoi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Loai",
                schema: "tMasterData",
                table: "ToHoi",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 8, 7, 21, 27, 43, 745, DateTimeKind.Local).AddTicks(8384),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 8, 4, 14, 6, 13, 902, DateTimeKind.Local).AddTicks(122));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 8, 7, 21, 27, 43, 745, DateTimeKind.Local).AddTicks(6344),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 8, 4, 14, 6, 13, 901, DateTimeKind.Local).AddTicks(7730));

            migrationBuilder.AddColumn<string>(
                name: "Loai",
                schema: "tMasterData",
                table: "ChiHoi",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HinhAnh",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Loai",
                schema: "tMasterData",
                table: "ToHoi");

            migrationBuilder.DropColumn(
                name: "Loai",
                schema: "tMasterData",
                table: "ChiHoi");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 8, 4, 14, 6, 13, 902, DateTimeKind.Local).AddTicks(122),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 8, 7, 21, 27, 43, 745, DateTimeKind.Local).AddTicks(8384));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 8, 4, 14, 6, 13, 901, DateTimeKind.Local).AddTicks(7730),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 8, 7, 21, 27, 43, 745, DateTimeKind.Local).AddTicks(6344));

            migrationBuilder.AlterColumn<string>(
                name: "HinhAnh",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
