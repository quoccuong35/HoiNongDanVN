using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addHoiVienHoTro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoiVienVayVon",
                schema: "HV");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 10, 9, 9, 37, 25, 334, DateTimeKind.Local).AddTicks(1985),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 24, 10, 12, 29, 671, DateTimeKind.Local).AddTicks(5203));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 10, 9, 9, 37, 25, 333, DateTimeKind.Local).AddTicks(7962),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 24, 10, 12, 29, 671, DateTimeKind.Local).AddTicks(2811));

            migrationBuilder.CreateTable(
                name: "HinhThucHoTro",
                schema: "tMasterData",
                columns: table => new
                {
                    MaHinhThucHoTro = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenHinhThuc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhThucHoTro", x => x.MaHinhThucHoTro);
                });

            migrationBuilder.CreateTable(
                name: "NguonVon",
                schema: "tMasterData",
                columns: table => new
                {
                    MaNguonVon = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenNguonVon = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguonVon", x => x.MaNguonVon);
                });

            migrationBuilder.CreateTable(
                name: "HoiVienHoTro",
                schema: "HV",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDHoiVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoTienVay = table.Column<long>(type: "bigint", nullable: true),
                    ThoiHangChoVay = table.Column<int>(type: "int", nullable: true),
                    LaiSuatVay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayTraNoCuoiCung = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TraXong = table.Column<bool>(type: "bit", nullable: false),
                    MaHinhThucHoTro = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HinhThucHoTroMaHinhThucHoTro = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaNguonVon = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NguonVonMaNguonVon = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoiVienHoTro", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HoiVienHoTro_HinhThucHoTro_HinhThucHoTroMaHinhThucHoTro",
                        column: x => x.HinhThucHoTroMaHinhThucHoTro,
                        principalSchema: "tMasterData",
                        principalTable: "HinhThucHoTro",
                        principalColumn: "MaHinhThucHoTro",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoiVienHoTro_NguonVon_NguonVonMaNguonVon",
                        column: x => x.NguonVonMaNguonVon,
                        principalSchema: "tMasterData",
                        principalTable: "NguonVon",
                        principalColumn: "MaNguonVon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoiVienVayVon_HoiVien",
                        column: x => x.IDHoiVien,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoiVienHoTro_HinhThucHoTroMaHinhThucHoTro",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "HinhThucHoTroMaHinhThucHoTro");

            migrationBuilder.CreateIndex(
                name: "IX_HoiVienHoTro_IDHoiVien",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "IDHoiVien");

            migrationBuilder.CreateIndex(
                name: "IX_HoiVienHoTro_NguonVonMaNguonVon",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "NguonVonMaNguonVon");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoiVienHoTro",
                schema: "HV");

            migrationBuilder.DropTable(
                name: "HinhThucHoTro",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "NguonVon",
                schema: "tMasterData");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 24, 10, 12, 29, 671, DateTimeKind.Local).AddTicks(5203),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 10, 9, 9, 37, 25, 334, DateTimeKind.Local).AddTicks(1985));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 24, 10, 12, 29, 671, DateTimeKind.Local).AddTicks(2811),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 10, 9, 9, 37, 25, 333, DateTimeKind.Local).AddTicks(7962));

            migrationBuilder.CreateTable(
                name: "HoiVienVayVon",
                schema: "HV",
                columns: table => new
                {
                    IDVayVon = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDCanBo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LaiSuatVay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayTraNoCuoiCung = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NoiDungVay = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SoTienVay = table.Column<long>(type: "bigint", nullable: false),
                    ThoiHangChoVay = table.Column<int>(type: "int", nullable: false),
                    TraXong = table.Column<bool>(type: "bit", nullable: false),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoiVienVayVon", x => x.IDVayVon);
                    table.ForeignKey(
                        name: "FK_HoiVienVayVon_HoiVien",
                        column: x => x.IDCanBo,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoiVienVayVon_IDCanBo",
                schema: "HV",
                table: "HoiVienVayVon",
                column: "IDCanBo");
        }
    }
}
