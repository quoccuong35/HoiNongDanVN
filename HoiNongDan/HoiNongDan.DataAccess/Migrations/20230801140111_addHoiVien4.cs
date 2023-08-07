using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addHoiVien4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_GiaDinhThuocDien_GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_NgheNghiep_NgheNghiepMaNgheNghiep",
                schema: "NS",
                table: "CanBo");

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
                name: "NgheNghiepMaNgheNghiep",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 1, 21, 1, 9, 337, DateTimeKind.Local).AddTicks(9003),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 1, 20, 48, 28, 375, DateTimeKind.Local).AddTicks(927));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 1, 21, 1, 9, 337, DateTimeKind.Local).AddTicks(2034),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 1, 20, 48, 28, 374, DateTimeKind.Local).AddTicks(6919));

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo",
                column: "MaGiaDinhThuocDien");

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaNgheNghiep",
                schema: "NS",
                table: "CanBo",
                column: "MaNgheNghiep");

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_MaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo",
                column: "MaGiaDinhThuocDien",
                principalSchema: "tMasterData",
                principalTable: "GiaDinhThuocDien",
                principalColumn: "MaGiaDinhThuocDien");

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_NgheNghiep",
                schema: "NS",
                table: "CanBo",
                column: "MaNgheNghiep",
                principalSchema: "tMasterData",
                principalTable: "NgheNghiep",
                principalColumn: "MaNgheNghiep");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_MaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_NgheNghiep",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropIndex(
                name: "IX_CanBo_MaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropIndex(
                name: "IX_CanBo_MaNgheNghiep",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 1, 20, 48, 28, 375, DateTimeKind.Local).AddTicks(927),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 1, 21, 1, 9, 337, DateTimeKind.Local).AddTicks(9003));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 1, 20, 48, 28, 374, DateTimeKind.Local).AddTicks(6919),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 1, 21, 1, 9, 337, DateTimeKind.Local).AddTicks(2034));

            migrationBuilder.AddColumn<string>(
                name: "GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NgheNghiepMaNgheNghiep",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(10)",
                nullable: true);

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
    }
}
