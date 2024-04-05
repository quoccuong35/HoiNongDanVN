using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class EditQuaTrinhKhenThuongAdd_Nam_Loai : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LyDo",
                schema: "NS",
                table: "QuaTrinhKhenThuong");

            migrationBuilder.AlterColumn<string>(
                name: "NoiDung",
                schema: "NS",
                table: "QuaTrinhKhenThuong",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Loai",
                schema: "NS",
                table: "QuaTrinhKhenThuong",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Nam",
                schema: "NS",
                table: "QuaTrinhKhenThuong",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 27, 19, 58, 36, 821, DateTimeKind.Local).AddTicks(8269),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 20, 13, 13, 19, 804, DateTimeKind.Local).AddTicks(8376));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 27, 19, 58, 36, 821, DateTimeKind.Local).AddTicks(5286),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 20, 13, 13, 19, 804, DateTimeKind.Local).AddTicks(5830));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Loai",
                schema: "NS",
                table: "QuaTrinhKhenThuong");

            migrationBuilder.DropColumn(
                name: "Nam",
                schema: "NS",
                table: "QuaTrinhKhenThuong");

            migrationBuilder.AlterColumn<string>(
                name: "NoiDung",
                schema: "NS",
                table: "QuaTrinhKhenThuong",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LyDo",
                schema: "NS",
                table: "QuaTrinhKhenThuong",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 20, 13, 13, 19, 804, DateTimeKind.Local).AddTicks(8376),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 27, 19, 58, 36, 821, DateTimeKind.Local).AddTicks(8269));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 20, 13, 13, 19, 804, DateTimeKind.Local).AddTicks(5830),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 27, 19, 58, 36, 821, DateTimeKind.Local).AddTicks(5286));
        }
    }
}
