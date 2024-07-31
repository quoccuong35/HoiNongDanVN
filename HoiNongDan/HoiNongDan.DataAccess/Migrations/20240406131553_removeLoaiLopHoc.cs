using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class removeLoaiLopHoc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoiVienHoTro_HinhThucHoTro",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropForeignKey(
                name: "FK_LopHoc_LoaiLopHoc",
                schema: "tMasterData",
                table: "LopHoc");

            migrationBuilder.DropTable(
                name: "LoaiLopHoc",
                schema: "tMasterData");

            migrationBuilder.DropIndex(
                name: "IX_HoiVienHoTro_MaHinhThucHoTro",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.DropColumn(
                name: "MaHinhThucHoTro",
                schema: "HV",
                table: "HoiVienHoTro");

            migrationBuilder.RenameColumn(
                name: "IDLoaiLopHoc",
                schema: "tMasterData",
                table: "LopHoc",
                newName: "MaHinhThucHoTro");

            migrationBuilder.RenameIndex(
                name: "IX_LopHoc_IDLoaiLopHoc",
                schema: "tMasterData",
                table: "LopHoc",
                newName: "IX_LopHoc_MaHinhThucHoTro");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 4, 6, 20, 15, 51, 348, DateTimeKind.Local).AddTicks(377),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 4, 6, 13, 50, 24, 538, DateTimeKind.Local).AddTicks(6506));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 4, 6, 20, 15, 51, 347, DateTimeKind.Local).AddTicks(8185),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 4, 6, 13, 50, 24, 538, DateTimeKind.Local).AddTicks(3427));

            migrationBuilder.AddForeignKey(
                name: "FK_LopHoc_HinhThucHoTro",
                schema: "tMasterData",
                table: "LopHoc",
                column: "MaHinhThucHoTro",
                principalSchema: "tMasterData",
                principalTable: "HinhThucHoTro",
                principalColumn: "MaHinhThucHoTro",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LopHoc_HinhThucHoTro",
                schema: "tMasterData",
                table: "LopHoc");

            migrationBuilder.RenameColumn(
                name: "MaHinhThucHoTro",
                schema: "tMasterData",
                table: "LopHoc",
                newName: "IDLoaiLopHoc");

            migrationBuilder.RenameIndex(
                name: "IX_LopHoc_MaHinhThucHoTro",
                schema: "tMasterData",
                table: "LopHoc",
                newName: "IX_LopHoc_IDLoaiLopHoc");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 4, 6, 13, 50, 24, 538, DateTimeKind.Local).AddTicks(6506),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 4, 6, 20, 15, 51, 348, DateTimeKind.Local).AddTicks(377));

            migrationBuilder.AddColumn<Guid>(
                name: "MaHinhThucHoTro",
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
                defaultValue: new DateTime(2024, 4, 6, 13, 50, 24, 538, DateTimeKind.Local).AddTicks(3427),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 4, 6, 20, 15, 51, 347, DateTimeKind.Local).AddTicks(8185));

            migrationBuilder.CreateTable(
                name: "LoaiLopHoc",
                schema: "tMasterData",
                columns: table => new
                {
                    IDLoaiLopHoc = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: true),
                    TenLoaiLopHoc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiLopHoc", x => x.IDLoaiLopHoc);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoiVienHoTro_MaHinhThucHoTro",
                schema: "HV",
                table: "HoiVienHoTro",
                column: "MaHinhThucHoTro");

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
                name: "FK_LopHoc_LoaiLopHoc",
                schema: "tMasterData",
                table: "LopHoc",
                column: "IDLoaiLopHoc",
                principalSchema: "tMasterData",
                principalTable: "LoaiLopHoc",
                principalColumn: "IDLoaiLopHoc",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
