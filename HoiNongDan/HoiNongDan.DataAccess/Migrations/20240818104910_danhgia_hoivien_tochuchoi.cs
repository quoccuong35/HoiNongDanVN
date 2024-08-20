using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class danhgia_hoivien_tochuchoi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 8, 18, 17, 49, 9, 290, DateTimeKind.Local).AddTicks(6017),
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
                defaultValue: new DateTime(2024, 8, 18, 17, 49, 9, 290, DateTimeKind.Local).AddTicks(4053),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 8, 7, 21, 27, 43, 745, DateTimeKind.Local).AddTicks(6344));

            migrationBuilder.CreateTable(
                name: "DanhGiaHoiVien",
                schema: "HV",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDHoiVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nam = table.Column<int>(type: "int", nullable: false),
                    LoaiDanhGia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGiaHoiVien", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DanhGiaHoiVien_HoiVien",
                        column: x => x.IDHoiVien,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DanhGiaToChucHoi",
                schema: "HV",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDDiaBanHoi = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoaiToChuc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nam = table.Column<int>(type: "int", nullable: false),
                    LoaiDanhGia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGiaToChucHoi", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DanhGiaToChucHoi_DiaBanHoatDong",
                        column: x => x.IDDiaBanHoi,
                        principalSchema: "HV",
                        principalTable: "DiaBanHoatDong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaHoiVien_IDHoiVien",
                schema: "HV",
                table: "DanhGiaHoiVien",
                column: "IDHoiVien");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaToChucHoi_IDDiaBanHoi",
                schema: "HV",
                table: "DanhGiaToChucHoi",
                column: "IDDiaBanHoi");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoiVienLichSuDuyet_HoiVien",
                schema: "HV",
                table: "HoiVienLichSuDuyet");

            migrationBuilder.DropTable(
                name: "DanhGiaHoiVien",
                schema: "HV");

            migrationBuilder.DropTable(
                name: "DanhGiaToChucHoi",
                schema: "HV");

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
                oldDefaultValue: new DateTime(2024, 8, 18, 17, 49, 9, 290, DateTimeKind.Local).AddTicks(6017));

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
                oldDefaultValue: new DateTime(2024, 8, 18, 17, 49, 9, 290, DateTimeKind.Local).AddTicks(4053));
        }
    }
}
