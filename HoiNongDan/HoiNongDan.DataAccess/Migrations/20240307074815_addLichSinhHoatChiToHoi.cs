using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addLichSinhHoatChiToHoi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 7, 14, 48, 14, 524, DateTimeKind.Local).AddTicks(1967),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 6, 15, 31, 4, 497, DateTimeKind.Local).AddTicks(6));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 7, 14, 48, 14, 523, DateTimeKind.Local).AddTicks(9404),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 6, 15, 31, 4, 496, DateTimeKind.Local).AddTicks(7793));

            migrationBuilder.CreateTable(
                name: "LichSinhHoatChiToHoi",
                schema: "HV",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDDiaBanHoiVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDMaChiToHoi = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenNoiDungSinhHoat = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Ngay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NoiDungSinhHoat = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SoLuongNguoiThanGia = table.Column<int>(type: "int", nullable: false),
                    ID_LSH_TPTG = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSinhHoatChiToHoi", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LichSinhHoatChiToHoi_DiaBanHoatDong",
                        column: x => x.IDDiaBanHoiVien,
                        principalSchema: "HV",
                        principalTable: "DiaBanHoatDong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LichSinhHoatChiToHoi_NguoiThamGia",
                schema: "HV",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaHoiVien = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenHoiVien = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ChucVu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IDLichSinhHoatChiToHoi = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSinhHoatChiToHoi_NguoiThamGia", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LichSinhHoatChiToHoi_NguoiThamGia_LichSinhHoatChiToHoi",
                        column: x => x.IDLichSinhHoatChiToHoi,
                        principalSchema: "HV",
                        principalTable: "LichSinhHoatChiToHoi",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LichSinhHoatChiToHoi_IDDiaBanHoiVien",
                schema: "HV",
                table: "LichSinhHoatChiToHoi",
                column: "IDDiaBanHoiVien");

            migrationBuilder.CreateIndex(
                name: "IX_LichSinhHoatChiToHoi_NguoiThamGia_IDLichSinhHoatChiToHoi",
                schema: "HV",
                table: "LichSinhHoatChiToHoi_NguoiThamGia",
                column: "IDLichSinhHoatChiToHoi");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LichSinhHoatChiToHoi_NguoiThamGia",
                schema: "HV");

            migrationBuilder.DropTable(
                name: "LichSinhHoatChiToHoi",
                schema: "HV");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 6, 15, 31, 4, 497, DateTimeKind.Local).AddTicks(6),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 7, 14, 48, 14, 524, DateTimeKind.Local).AddTicks(1967));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 6, 15, 31, 4, 496, DateTimeKind.Local).AddTicks(7793),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 7, 14, 48, 14, 523, DateTimeKind.Local).AddTicks(9404));
        }
    }
}
