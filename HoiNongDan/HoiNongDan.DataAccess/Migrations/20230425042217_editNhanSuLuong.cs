using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editNhanSuLuong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 25, 11, 22, 16, 704, DateTimeKind.Local).AddTicks(2028),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 23, 19, 23, 1, 643, DateTimeKind.Local).AddTicks(5385));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 25, 11, 22, 16, 703, DateTimeKind.Local).AddTicks(7278),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 23, 19, 23, 1, 643, DateTimeKind.Local).AddTicks(1351));

            migrationBuilder.AddColumn<DateTime>(
                name: "KhoanDenNgay",
                schema: "NS",
                table: "CanBo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "KhoanTuNgay",
                schema: "NS",
                table: "CanBo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaSoThue",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SoBHXH",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SoBHYT",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PhanHeModel",
                schema: "tMasterData",
                columns: table => new
                {
                    MaPhanHe = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenPhanHe = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhanHeModel", x => x.MaPhanHe);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhanHeModel",
                schema: "tMasterData");

            migrationBuilder.DropColumn(
                name: "KhoanDenNgay",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "KhoanTuNgay",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "MaSoThue",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "SoBHXH",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "SoBHYT",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 23, 19, 23, 1, 643, DateTimeKind.Local).AddTicks(5385),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 25, 11, 22, 16, 704, DateTimeKind.Local).AddTicks(2028));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 23, 19, 23, 1, 643, DateTimeKind.Local).AddTicks(1351),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 25, 11, 22, 16, 703, DateTimeKind.Local).AddTicks(7278));
        }
    }
}
