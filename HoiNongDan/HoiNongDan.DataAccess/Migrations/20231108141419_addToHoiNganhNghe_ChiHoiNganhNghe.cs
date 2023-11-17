﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addToHoiNganhNghe_ChiHoiNganhNghe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 8, 21, 14, 17, 377, DateTimeKind.Local).AddTicks(2287),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 7, 19, 32, 50, 194, DateTimeKind.Local).AddTicks(9154));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 8, 21, 14, 17, 376, DateTimeKind.Local).AddTicks(8329),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 7, 19, 32, 50, 194, DateTimeKind.Local).AddTicks(4870));

            migrationBuilder.CreateTable(
                name: "ToHoiNganhNghe_ChiHoiNganhNghe",
                schema: "tMasterData",
                columns: table => new
                {
                    Ma_ToHoiNganhNghe_ChiHoiNganhNghe = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToHoiNganhNghe_ChiHoiNganhNghe", x => x.Ma_ToHoiNganhNghe_ChiHoiNganhNghe);
                });

            migrationBuilder.CreateTable(
                name: "ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien",
                schema: "HV",
                columns: table => new
                {
                    IDHoiVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ma_ToHoiNganhNghe_ChiHoiNganhNghe = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien", x => new { x.Ma_ToHoiNganhNghe_ChiHoiNganhNghe, x.IDHoiVien });
                    table.ForeignKey(
                        name: "FK_ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien",
                        column: x => x.IDHoiVien,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToHoiNganhNghe_ChiHoiNganhNghe_ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien",
                        column: x => x.Ma_ToHoiNganhNghe_ChiHoiNganhNghe,
                        principalSchema: "tMasterData",
                        principalTable: "ToHoiNganhNghe_ChiHoiNganhNghe",
                        principalColumn: "Ma_ToHoiNganhNghe_ChiHoiNganhNghe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien_IDHoiVien",
                schema: "HV",
                table: "ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien",
                column: "IDHoiVien");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToHoiNganhNghe_ChiHoiNganhNghe_HoiVien",
                schema: "HV");

            migrationBuilder.DropTable(
                name: "ToHoiNganhNghe_ChiHoiNganhNghe",
                schema: "tMasterData");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 7, 19, 32, 50, 194, DateTimeKind.Local).AddTicks(9154),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 8, 21, 14, 17, 377, DateTimeKind.Local).AddTicks(2287));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 7, 19, 32, 50, 194, DateTimeKind.Local).AddTicks(4870),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 8, 21, 14, 17, 376, DateTimeKind.Local).AddTicks(8329));
        }
    }
}
