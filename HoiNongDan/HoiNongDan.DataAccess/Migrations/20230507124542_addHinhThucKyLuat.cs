using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addHinhThucKyLuat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 7, 19, 45, 41, 282, DateTimeKind.Local).AddTicks(7175),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 7, 11, 23, 54, 285, DateTimeKind.Local).AddTicks(4366));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 7, 19, 45, 41, 282, DateTimeKind.Local).AddTicks(3371),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 7, 11, 23, 54, 284, DateTimeKind.Local).AddTicks(6766));

            migrationBuilder.CreateTable(
                name: "HinhThucKyLuat",
                schema: "tMasterData",
                columns: table => new
                {
                    MaHinhThucKyLuat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenHinhThucKyLuat = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DinhChi = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhThucKyLuat", x => x.MaHinhThucKyLuat);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HinhThucKyLuat",
                schema: "tMasterData");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 7, 11, 23, 54, 285, DateTimeKind.Local).AddTicks(4366),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 7, 19, 45, 41, 282, DateTimeKind.Local).AddTicks(7175));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 7, 11, 23, 54, 284, DateTimeKind.Local).AddTicks(6766),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 7, 19, 45, 41, 282, DateTimeKind.Local).AddTicks(3371));
        }
    }
}
