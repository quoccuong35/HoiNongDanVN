using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class AddVayVon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoiVienHoTro_NguonVon",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropIndex(
                name: "IX_HoiVienHoTro_MaNguonVon",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropColumn(
                name: "MaNguonVon",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 4, 6, 13, 50, 24, 538, DateTimeKind.Local).AddTicks(6506),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 4, 3, 21, 11, 41, 829, DateTimeKind.Local).AddTicks(1675));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 4, 6, 13, 50, 24, 538, DateTimeKind.Local).AddTicks(3427),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 4, 3, 21, 11, 41, 828, DateTimeKind.Local).AddTicks(9198));

            migrationBuilder.CreateTable(
                name: "LoaiLopHoc",
                schema: "tMasterData",
                columns: table => new
                {
                    IDLoaiLopHoc = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenLoaiLopHoc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiLopHoc", x => x.IDLoaiLopHoc);
                });

            migrationBuilder.CreateTable(
                name: "VayVon",
                schema: "HV",
                columns: table => new
                {
                    IDVayVon = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDHoiVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoTienVay = table.Column<long>(type: "bigint", nullable: true),
                    ThoiHangChoVay = table.Column<int>(type: "int", nullable: true),
                    LaiSuatVay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayTraNoCuoiCung = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TraXong = table.Column<bool>(type: "bit", nullable: false),
                    MaNguonVon = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VayVon", x => x.IDVayVon);
                    table.ForeignKey(
                        name: "FK_VayVon_NguonVon",
                        column: x => x.MaNguonVon,
                        principalSchema: "tMasterData",
                        principalTable: "NguonVon",
                        principalColumn: "MaNguonVon");
                    table.ForeignKey(
                        name: "FK_VayVons_HoiVien",
                        column: x => x.IDHoiVien,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LopHoc",
                schema: "tMasterData",
                columns: table => new
                {
                    IDLopHoc = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenLopHoc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IDLoaiLopHoc = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LopHoc", x => x.IDLopHoc);
                    table.ForeignKey(
                        name: "FK_LopHoc_LoaiLopHoc",
                        column: x => x.IDLoaiLopHoc,
                        principalSchema: "tMasterData",
                        principalTable: "LoaiLopHoc",
                        principalColumn: "IDLoaiLopHoc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LopHoc_IDLoaiLopHoc",
                schema: "tMasterData",
                table: "LopHoc",
                column: "IDLoaiLopHoc");

            migrationBuilder.CreateIndex(
                name: "IX_VayVon_IDHoiVien",
                schema: "HV",
                table: "VayVon",
                column: "IDHoiVien");

            migrationBuilder.CreateIndex(
                name: "IX_VayVon_MaNguonVon",
                schema: "HV",
                table: "VayVon",
                column: "MaNguonVon");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LopHoc",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "VayVon",
                schema: "HV");

            migrationBuilder.DropTable(
                name: "LoaiLopHoc",
                schema: "tMasterData");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 4, 3, 21, 11, 41, 829, DateTimeKind.Local).AddTicks(1675),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 4, 6, 13, 50, 24, 538, DateTimeKind.Local).AddTicks(6506));

            migrationBuilder.AddColumn<Guid>(
                name: "MaNguonVon",
                schema: "HV",
                table: "HoiVienHoTro",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 4, 3, 21, 11, 41, 828, DateTimeKind.Local).AddTicks(9198),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 4, 6, 13, 50, 24, 538, DateTimeKind.Local).AddTicks(3427));

            migrationBuilder.CreateIndex(
                name: "IX_HoiVienHoTro_MaNguonVon",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "MaNguonVon");

            migrationBuilder.AddForeignKey(
                name: "FK_HoiVienHoTro_NguonVon",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "MaNguonVon",
                principalSchema: "tMasterData",
                principalTable: "NguonVon",
                principalColumn: "MaNguonVon");
        }
    }
}
