using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class EditCanBoDandVien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 26, 17, 4, 37, 846, DateTimeKind.Local).AddTicks(8552),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 26, 15, 31, 21, 34, DateTimeKind.Local).AddTicks(9178));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 26, 17, 4, 37, 846, DateTimeKind.Local).AddTicks(5902),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 26, 15, 31, 21, 34, DateTimeKind.Local).AddTicks(6844));

            migrationBuilder.AddColumn<bool>(
                name: "DKMauNguoiNongDanMoi",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DangVien",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "KKAnToanThucPham",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DKMauNguoiNongDanMoi",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "DangVien",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "KKAnToanThucPham",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 26, 15, 31, 21, 34, DateTimeKind.Local).AddTicks(9178),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 26, 17, 4, 37, 846, DateTimeKind.Local).AddTicks(8552));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 26, 15, 31, 21, 34, DateTimeKind.Local).AddTicks(6844),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 26, 17, 4, 37, 846, DateTimeKind.Local).AddTicks(5902));
        }
    }
}
