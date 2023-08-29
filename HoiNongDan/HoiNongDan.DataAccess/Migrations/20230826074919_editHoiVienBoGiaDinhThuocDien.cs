using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editHoiVienBoGiaDinhThuocDien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_ChucVu",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_GiaDinhThuocDien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropIndex(
                name: "IX_CanBo_MaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "DienTich",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "MaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.RenameColumn(
                name: "SoLuong",
                schema: "NS",
                table: "CanBo",
                newName: "DienTich_QuyMo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 26, 14, 49, 17, 826, DateTimeKind.Local).AddTicks(168),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 25, 10, 35, 31, 896, DateTimeKind.Local).AddTicks(5712));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 26, 14, 49, 17, 825, DateTimeKind.Local).AddTicks(7918),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 25, 10, 35, 31, 896, DateTimeKind.Local).AddTicks(421));

            migrationBuilder.AlterColumn<string>(
                name: "SoCCCD",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NgaySinh",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaChucVu",
                schema: "NS",
                table: "CanBo",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<bool>(
                name: "CanNgheo",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GiaDinhChinhSach",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HoNgheo",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo",
                column: "GiaDinhThuocDienMaGiaDinhThuocDien");

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_ChucVu",
                schema: "NS",
                table: "CanBo",
                column: "MaChucVu",
                principalSchema: "tMasterData",
                principalTable: "ChucVuModel",
                principalColumn: "MaChucVu");

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_GiaDinhThuocDien_GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo",
                column: "GiaDinhThuocDienMaGiaDinhThuocDien",
                principalSchema: "tMasterData",
                principalTable: "GiaDinhThuocDien",
                principalColumn: "MaGiaDinhThuocDien");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_ChucVu",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropForeignKey(
                name: "FK_CanBo_GiaDinhThuocDien_GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropIndex(
                name: "IX_CanBo_GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "CanNgheo",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "GiaDinhChinhSach",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "GiaDinhThuocDienMaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "HoNgheo",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.RenameColumn(
                name: "DienTich_QuyMo",
                schema: "NS",
                table: "CanBo",
                newName: "SoLuong");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 25, 10, 35, 31, 896, DateTimeKind.Local).AddTicks(5712),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 26, 14, 49, 17, 826, DateTimeKind.Local).AddTicks(168));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 25, 10, 35, 31, 896, DateTimeKind.Local).AddTicks(421),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 26, 14, 49, 17, 825, DateTimeKind.Local).AddTicks(7918));

            migrationBuilder.AlterColumn<string>(
                name: "SoCCCD",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NgaySinh",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MaChucVu",
                schema: "NS",
                table: "CanBo",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DienTich",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CanBo_MaGiaDinhThuocDien",
                schema: "NS",
                table: "CanBo",
                column: "MaGiaDinhThuocDien");

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_ChucVu",
                schema: "NS",
                table: "CanBo",
                column: "MaChucVu",
                principalSchema: "tMasterData",
                principalTable: "ChucVuModel",
                principalColumn: "MaChucVu",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CanBo_GiaDinhThuocDien",
                schema: "NS",
                table: "CanBo",
                column: "MaGiaDinhThuocDien",
                principalSchema: "tMasterData",
                principalTable: "GiaDinhThuocDien",
                principalColumn: "MaGiaDinhThuocDien");
        }
    }
}
