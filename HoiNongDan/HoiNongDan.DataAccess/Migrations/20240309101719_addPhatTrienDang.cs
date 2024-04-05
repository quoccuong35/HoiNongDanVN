using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addPhatTrienDang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 9, 17, 17, 17, 887, DateTimeKind.Local).AddTicks(2724),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 8, 15, 30, 57, 999, DateTimeKind.Local).AddTicks(2191));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 9, 17, 17, 17, 886, DateTimeKind.Local).AddTicks(484),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 8, 15, 30, 57, 998, DateTimeKind.Local).AddTicks(9672));

            migrationBuilder.CreateTable(
                name: "PhatTrienDang",
                schema: "HV",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenVietTat = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaDiaBanHoiND = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhatTrienDang", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PhatTrienDang_DiaBanHoatDong",
                        column: x => x.MaDiaBanHoiND,
                        principalSchema: "HV",
                        principalTable: "DiaBanHoatDong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhatTrienDang_HoiVien",
                schema: "HV",
                columns: table => new
                {
                    IDHoiVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDPhatTrienDang = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhatTrienDang_HoiVien", x => new { x.IDPhatTrienDang, x.IDHoiVien });
                    table.ForeignKey(
                        name: "FK_CanBo_PhatTrienDang_HoiVien",
                        column: x => x.IDHoiVien,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhatTrienDang_PhatTrienDang_HoiVien",
                        column: x => x.IDPhatTrienDang,
                        principalSchema: "HV",
                        principalTable: "PhatTrienDang",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhatTrienDang_MaDiaBanHoiND",
                schema: "HV",
                table: "PhatTrienDang",
                column: "MaDiaBanHoiND");

            migrationBuilder.CreateIndex(
                name: "IX_PhatTrienDang_HoiVien_IDHoiVien",
                schema: "HV",
                table: "PhatTrienDang_HoiVien",
                column: "IDHoiVien");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhatTrienDang_HoiVien",
                schema: "HV");

            migrationBuilder.DropTable(
                name: "PhatTrienDang",
                schema: "HV");

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
                oldDefaultValue: new DateTime(2024, 3, 9, 17, 17, 17, 887, DateTimeKind.Local).AddTicks(2724));

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
                oldDefaultValue: new DateTime(2024, 3, 9, 17, 17, 17, 886, DateTimeKind.Local).AddTicks(484));
        }
    }
}
