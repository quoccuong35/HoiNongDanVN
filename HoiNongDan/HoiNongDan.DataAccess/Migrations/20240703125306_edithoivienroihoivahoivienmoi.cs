﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class edithoivienroihoivahoivienmoi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
          

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 3, 19, 53, 4, 718, DateTimeKind.Local).AddTicks(602),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 4, 8, 20, 0, 3, 882, DateTimeKind.Local).AddTicks(8106));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 3, 19, 53, 4, 717, DateTimeKind.Local).AddTicks(7714),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 4, 8, 20, 0, 3, 882, DateTimeKind.Local).AddTicks(5774));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoiVienDanCu",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "HoiVienNganhNghe",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "KhongDuyet",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "LyDoKhongDuyet",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "NgayCapThe",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.RenameColumn(
                name: "NgayRoiHoi",
                schema: "NS",
                table: "CanBo",
                newName: "NgayNgungHoatDong");

            migrationBuilder.RenameColumn(
                name: "LyDoRoiHoi",
                schema: "NS",
                table: "CanBo",
                newName: "LyDoNgungHoatDong");

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
                oldDefaultValue: new DateTime(2024, 7, 3, 19, 53, 4, 718, DateTimeKind.Local).AddTicks(602));

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
                oldDefaultValue: new DateTime(2024, 7, 3, 19, 53, 4, 717, DateTimeKind.Local).AddTicks(7714));
        }
    }
}
