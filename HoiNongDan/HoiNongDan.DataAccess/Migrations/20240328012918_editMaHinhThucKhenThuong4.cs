using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editMaHinhThucKhenThuong4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuanHeGiaDinh_HinhThucKhenThuong",
                schema: "NS",
                table: "QuaTrinhKhenThuong");

            migrationBuilder.AlterColumn<string>(
                name: "MaHinhThucKhenThuong",
                schema: "NS",
                table: "QuaTrinhKhenThuong",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 28, 8, 29, 17, 402, DateTimeKind.Local).AddTicks(3153),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 27, 20, 27, 46, 260, DateTimeKind.Local).AddTicks(5571));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 28, 8, 29, 17, 402, DateTimeKind.Local).AddTicks(135),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 27, 20, 27, 46, 260, DateTimeKind.Local).AddTicks(3237));

            migrationBuilder.AddForeignKey(
                name: "FK_QuanHeGiaDinh_HinhThucKhenThuong",
                schema: "NS",
                table: "QuaTrinhKhenThuong",
                column: "MaHinhThucKhenThuong",
                principalSchema: "tMasterData",
                principalTable: "HinhThucKhenThuong",
                principalColumn: "MaHinhThucKhenThuong");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuanHeGiaDinh_HinhThucKhenThuong",
                schema: "NS",
                table: "QuaTrinhKhenThuong");

            migrationBuilder.AlterColumn<string>(
                name: "MaHinhThucKhenThuong",
                schema: "NS",
                table: "QuaTrinhKhenThuong",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 27, 20, 27, 46, 260, DateTimeKind.Local).AddTicks(5571),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 28, 8, 29, 17, 402, DateTimeKind.Local).AddTicks(3153));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 27, 20, 27, 46, 260, DateTimeKind.Local).AddTicks(3237),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 28, 8, 29, 17, 402, DateTimeKind.Local).AddTicks(135));

            migrationBuilder.AddForeignKey(
                name: "FK_QuanHeGiaDinh_HinhThucKhenThuong",
                schema: "NS",
                table: "QuaTrinhKhenThuong",
                column: "MaHinhThucKhenThuong",
                principalSchema: "tMasterData",
                principalTable: "HinhThucKhenThuong",
                principalColumn: "MaHinhThucKhenThuong",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
