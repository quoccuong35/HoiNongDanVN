using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editNguonHoTro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoiVienHoTro_NguonVon",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 10, 9, 15, 7, 56, 410, DateTimeKind.Local).AddTicks(1033),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 10, 9, 9, 41, 31, 878, DateTimeKind.Local).AddTicks(3156));

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguonVon",
                schema: "HV",
                table: "HoiVienHoTro",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 10, 9, 15, 7, 56, 409, DateTimeKind.Local).AddTicks(8787),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 10, 9, 9, 41, 31, 877, DateTimeKind.Local).AddTicks(9987));

            migrationBuilder.AddForeignKey(
                name: "FK_HoiVienHoTro_NguonVon",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "MaNguonVon",
                principalSchema: "tMasterData",
                principalTable: "NguonVon",
                principalColumn: "MaNguonVon");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoiVienHoTro_NguonVon",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 10, 9, 9, 41, 31, 878, DateTimeKind.Local).AddTicks(3156),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 10, 9, 15, 7, 56, 410, DateTimeKind.Local).AddTicks(1033));

            migrationBuilder.AlterColumn<Guid>(
                name: "MaNguonVon",
                schema: "HV",
                table: "HoiVienHoTro",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 10, 9, 9, 41, 31, 877, DateTimeKind.Local).AddTicks(9987),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 10, 9, 15, 7, 56, 409, DateTimeKind.Local).AddTicks(8787));

            migrationBuilder.AddForeignKey(
                name: "FK_HoiVienHoTro_NguonVon",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "MaNguonVon",
                principalSchema: "tMasterData",
                principalTable: "NguonVon",
                principalColumn: "MaNguonVon",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
