using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editHoiVienHoTro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoiVienHoTro_HinhThucHoTro_HinhThucHoTroMaHinhThucHoTro",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropForeignKey(
                name: "FK_HoiVienHoTro_NguonVon_NguonVonMaNguonVon",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropIndex(
                name: "IX_HoiVienHoTro_HinhThucHoTroMaHinhThucHoTro",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropIndex(
                name: "IX_HoiVienHoTro_NguonVonMaNguonVon",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropColumn(
                name: "HinhThucHoTroMaHinhThucHoTro",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropColumn(
                name: "NguonVonMaNguonVon",
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
                oldDefaultValue: new DateTime(2023, 10, 9, 9, 37, 25, 334, DateTimeKind.Local).AddTicks(1985));

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
                oldDefaultValue: new DateTime(2023, 10, 9, 9, 37, 25, 333, DateTimeKind.Local).AddTicks(7962));

            migrationBuilder.CreateIndex(
                name: "IX_HoiVienHoTro_MaHinhThucHoTro",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "MaHinhThucHoTro");

            migrationBuilder.CreateIndex(
                name: "IX_HoiVienHoTro_MaNguonVon",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "MaNguonVon");

            migrationBuilder.AddForeignKey(
                name: "FK_HoiVienHoTro_HinhThucHoTro",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "MaHinhThucHoTro",
                principalSchema: "tMasterData",
                principalTable: "HinhThucHoTro",
                principalColumn: "MaHinhThucHoTro",
                onDelete: ReferentialAction.Cascade);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoiVienHoTro_HinhThucHoTro",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropForeignKey(
                name: "FK_HoiVienHoTro_NguonVon",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropIndex(
                name: "IX_HoiVienHoTro_MaHinhThucHoTro",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropIndex(
                name: "IX_HoiVienHoTro_MaNguonVon",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 10, 9, 9, 37, 25, 334, DateTimeKind.Local).AddTicks(1985),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 10, 9, 9, 41, 31, 878, DateTimeKind.Local).AddTicks(3156));

            migrationBuilder.AddColumn<Guid>(
                name: "HinhThucHoTroMaHinhThucHoTro",
                schema: "HV",
                table: "HoiVienHoTro",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "NguonVonMaNguonVon",
                schema: "HV",
                table: "HoiVienHoTro",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 10, 9, 9, 37, 25, 333, DateTimeKind.Local).AddTicks(7962),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 10, 9, 9, 41, 31, 877, DateTimeKind.Local).AddTicks(9987));

            migrationBuilder.CreateIndex(
                name: "IX_HoiVienHoTro_HinhThucHoTroMaHinhThucHoTro",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "HinhThucHoTroMaHinhThucHoTro");

            migrationBuilder.CreateIndex(
                name: "IX_HoiVienHoTro_NguonVonMaNguonVon",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "NguonVonMaNguonVon");

            migrationBuilder.AddForeignKey(
                name: "FK_HoiVienHoTro_HinhThucHoTro_HinhThucHoTroMaHinhThucHoTro",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "HinhThucHoTroMaHinhThucHoTro",
                principalSchema: "tMasterData",
                principalTable: "HinhThucHoTro",
                principalColumn: "MaHinhThucHoTro",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HoiVienHoTro_NguonVon_NguonVonMaNguonVon",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "NguonVonMaNguonVon",
                principalSchema: "tMasterData",
                principalTable: "NguonVon",
                principalColumn: "MaNguonVon",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
