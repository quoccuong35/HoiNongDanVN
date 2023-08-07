using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addHoiVien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 1, 20, 40, 20, 263, DateTimeKind.Local).AddTicks(5723),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 25, 19, 32, 50, 210, DateTimeKind.Local).AddTicks(8205));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 1, 20, 40, 20, 263, DateTimeKind.Local).AddTicks(1562),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 25, 19, 32, 50, 210, DateTimeKind.Local).AddTicks(3442));

            migrationBuilder.AlterColumn<string>(
                name: "SoBHYT",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "SoBHXH",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoThue",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoKhauThuongTru",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaNgheNghiep",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayThamGiaCapUyDang",
                schema: "NS",
                table: "CanBo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayThamGiaHDND",
                schema: "NS",
                table: "CanBo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayVaoHoi",
                schema: "NS",
                table: "CanBo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NgheNghiepMaNgheNghiep",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VaiTro",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GiaDinhThuocDien",
                schema: "tMasterData",
                columns: table => new
                {
                    MaGiaDinhThuocDien = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenGiaDinhThuocDien = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiaDinhThuocDien", x => x.MaGiaDinhThuocDien);
                });

            migrationBuilder.CreateTable(
                name: "NgheNghiep",
                schema: "tMasterData",
                columns: table => new
                {
                    MaNgheNghiep = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenNgheNghiep = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NgheNghiep", x => x.MaNgheNghiep);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo",
                column: "GiaDinhThuocDienMaGiaDinhThuocDien");

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_NgheNghiepMaNgheNghiep",
                schema: "NS",
                table: "CanBo",
                column: "NgheNghiepMaNgheNghiep");

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_GiaDinhThuocDien_GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo",
                column: "GiaDinhThuocDienMaGiaDinhThuocDien",
                principalSchema: "tMasterData",
                principalTable: "GiaDinhThuocDien",
                principalColumn: "MaGiaDinhThuocDien");

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_NgheNghiep_NgheNghiepMaNgheNghiep",
                schema: "NS",
                table: "CanBo",
                column: "NgheNghiepMaNgheNghiep",
                principalSchema: "tMasterData",
                principalTable: "NgheNghiep",
                principalColumn: "MaNgheNghiep");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_GiaDinhThuocDien_GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_NgheNghiep_NgheNghiepMaNgheNghiep",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropTable(
                name: "GiaDinhThuocDien",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "NgheNghiep",
                schema: "tMasterData");

            migrationBuilder.DropIndex(
                name: "IX_CanBo_GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropIndex(
                name: "IX_CanBo_NgheNghiepMaNgheNghiep",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "HoKhauThuongTru",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "MaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "MaNgheNghiep",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "NgayThamGiaCapUyDang",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "NgayThamGiaHDND",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "NgayVaoHoi",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "NgheNghiepMaNgheNghiep",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "VaiTro",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 25, 19, 32, 50, 210, DateTimeKind.Local).AddTicks(8205),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 1, 20, 40, 20, 263, DateTimeKind.Local).AddTicks(5723));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 25, 19, 32, 50, 210, DateTimeKind.Local).AddTicks(3442),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 1, 20, 40, 20, 263, DateTimeKind.Local).AddTicks(1562));

            migrationBuilder.AlterColumn<string>(
                name: "SoBHYT",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SoBHXH",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoThue",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
