using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addHoiVien6 : Migration
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
                defaultValue: new DateTime(2023, 8, 1, 21, 10, 49, 699, DateTimeKind.Local).AddTicks(3186),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 1, 21, 7, 59, 211, DateTimeKind.Local).AddTicks(6260));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 1, 21, 10, 49, 698, DateTimeKind.Local).AddTicks(6932),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 1, 21, 7, 59, 211, DateTimeKind.Local).AddTicks(2388));

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
                name: "FK_CanBo_GiaDinhThuocDien",
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
                name: "FK_CanBo_GiaDinhThuocDien",
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
                defaultValue: new DateTime(2023, 8, 1, 21, 7, 59, 211, DateTimeKind.Local).AddTicks(6260),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 1, 21, 10, 49, 699, DateTimeKind.Local).AddTicks(3186));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 1, 21, 7, 59, 211, DateTimeKind.Local).AddTicks(2388),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 1, 21, 10, 49, 698, DateTimeKind.Local).AddTicks(6932));

            

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
