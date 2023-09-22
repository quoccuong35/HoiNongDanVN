using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editCanBoQHPX : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 19, 21, 13, 6, 91, DateTimeKind.Local).AddTicks(801),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 19, 7, 42, 36, 443, DateTimeKind.Local).AddTicks(7763));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 19, 21, 13, 6, 90, DateTimeKind.Local).AddTicks(8484),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 19, 7, 42, 36, 443, DateTimeKind.Local).AddTicks(5200));

            migrationBuilder.AddColumn<string>(
                name: "DangUyVien",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DanhGiaCBCC",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DanhGiaDangVien",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HDNNCapHuyen",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HDNNCapXa",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HuyenUyVien",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Level",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThamGiaBTV",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DangUyVien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "DanhGiaCBCC",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "DanhGiaDangVien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "HDNNCapHuyen",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "HDNNCapXa",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "HuyenUyVien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "Level",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "ThamGiaBTV",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 19, 7, 42, 36, 443, DateTimeKind.Local).AddTicks(7763),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 19, 21, 13, 6, 91, DateTimeKind.Local).AddTicks(801));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 19, 7, 42, 36, 443, DateTimeKind.Local).AddTicks(5200),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 19, 21, 13, 6, 90, DateTimeKind.Local).AddTicks(8484));
        }
    }
}
