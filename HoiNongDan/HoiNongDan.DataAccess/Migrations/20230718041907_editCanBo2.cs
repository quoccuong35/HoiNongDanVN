using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editCanBo2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HVHoKhauThuongTru",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "HVNgayVaoHoi",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "HVQuanHeChuHo",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "IsCanNgheo",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "IsChuHo",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "IsCongChucVienChuc",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "IsCongDan",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "IsDoanhNghiep",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "IsGiaDinhChinhSach",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "IsHoNgheo",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "IsHocSinhSinhVien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "IsHuuTri",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "IsLaoDongTuDo",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "IsNongDan",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "IsThanhPhanKhac",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 18, 11, 19, 5, 947, DateTimeKind.Local).AddTicks(7062),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 18, 10, 25, 28, 125, DateTimeKind.Local).AddTicks(6194));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 18, 11, 19, 5, 947, DateTimeKind.Local).AddTicks(3257),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 18, 10, 25, 28, 125, DateTimeKind.Local).AddTicks(2217));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 18, 10, 25, 28, 125, DateTimeKind.Local).AddTicks(6194),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 18, 11, 19, 5, 947, DateTimeKind.Local).AddTicks(7062));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 7, 18, 10, 25, 28, 125, DateTimeKind.Local).AddTicks(2217),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 7, 18, 11, 19, 5, 947, DateTimeKind.Local).AddTicks(3257));

            migrationBuilder.AddColumn<string>(
                name: "HVHoKhauThuongTru",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HVNgayVaoHoi",
                schema: "NS",
                table: "CanBo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HVQuanHeChuHo",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanNgheo",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsChuHo",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCongChucVienChuc",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCongDan",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDoanhNghiep",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGiaDinhChinhSach",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHoNgheo",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHocSinhSinhVien",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHuuTri",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLaoDongTuDo",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNongDan",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsThanhPhanKhac",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);
        }
    }
}
