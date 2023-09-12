using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class removeDaoTaoBoiDuong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuaTrinhBoiDuong_CanBo",
                schema: "NS",
                table: "QuaTrinhBoiDuong");

            migrationBuilder.DropForeignKey(
                name: "FK_QuaTrinhBoiDuong_HinhThucDaoTao",
                schema: "NS",
                table: "QuaTrinhBoiDuong");

            migrationBuilder.DropForeignKey(
                name: "FK_QuaTrinhDaoTao_ChuyenNganh",
                schema: "NS",
                table: "QuaTrinhDaoTao");

            migrationBuilder.DropForeignKey(
                name: "FK_QuaTrinhDaoTao_HinhThucDaoTao",
                schema: "NS",
                table: "QuaTrinhDaoTao");

            migrationBuilder.DropForeignKey(
                name: "FK_QuaTrinhDaoTao_LoaiBangCap",
                schema: "NS",
                table: "QuaTrinhDaoTao");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 9, 20, 11, 8, 133, DateTimeKind.Local).AddTicks(4622),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 8, 14, 6, 16, 93, DateTimeKind.Local).AddTicks(4925));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 9, 20, 11, 8, 133, DateTimeKind.Local).AddTicks(1932),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 8, 14, 6, 16, 92, DateTimeKind.Local).AddTicks(9567));

            migrationBuilder.AddForeignKey(
                name: "FK_QuaTrinhBoiDuong_CanBo",
                schema: "NS",
                table: "QuaTrinhBoiDuong",
                column: "IDCanBo",
                principalSchema: "NS",
                principalTable: "CanBo",
                principalColumn: "IDCanBo");

            migrationBuilder.AddForeignKey(
                name: "FK_QuaTrinhBoiDuong_HinhThucDaoTao",
                schema: "NS",
                table: "QuaTrinhBoiDuong",
                column: "MaHinhThucDaoTao",
                principalSchema: "tMasterData",
                principalTable: "HinhThucDaoTao",
                principalColumn: "MaHinhThucDaoTao");

            migrationBuilder.AddForeignKey(
                name: "FK_QuaTrinhDaoTao_ChuyenNganh",
                schema: "NS",
                table: "QuaTrinhDaoTao",
                column: "MaChuyenNganh",
                principalSchema: "tMasterData",
                principalTable: "ChuyenNganh",
                principalColumn: "MaChuyenNganh");

            migrationBuilder.AddForeignKey(
                name: "FK_QuaTrinhDaoTao_HinhThucDaoTao",
                schema: "NS",
                table: "QuaTrinhDaoTao",
                column: "MaHinhThucDaoTao",
                principalSchema: "tMasterData",
                principalTable: "HinhThucDaoTao",
                principalColumn: "MaHinhThucDaoTao");

            migrationBuilder.AddForeignKey(
                name: "FK_QuaTrinhDaoTao_LoaiBangCap",
                schema: "NS",
                table: "QuaTrinhDaoTao",
                column: "MaLoaiBangCap",
                principalSchema: "tMasterData",
                principalTable: "LoaiBangCap",
                principalColumn: "MaLoaiBangCap");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuaTrinhBoiDuong_CanBo",
                schema: "NS",
                table: "QuaTrinhBoiDuong");

            migrationBuilder.DropForeignKey(
                name: "FK_QuaTrinhBoiDuong_HinhThucDaoTao",
                schema: "NS",
                table: "QuaTrinhBoiDuong");

            migrationBuilder.DropForeignKey(
                name: "FK_QuaTrinhDaoTao_ChuyenNganh",
                schema: "NS",
                table: "QuaTrinhDaoTao");

            migrationBuilder.DropForeignKey(
                name: "FK_QuaTrinhDaoTao_HinhThucDaoTao",
                schema: "NS",
                table: "QuaTrinhDaoTao");

            migrationBuilder.DropForeignKey(
                name: "FK_QuaTrinhDaoTao_LoaiBangCap",
                schema: "NS",
                table: "QuaTrinhDaoTao");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 8, 14, 6, 16, 93, DateTimeKind.Local).AddTicks(4925),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 9, 20, 11, 8, 133, DateTimeKind.Local).AddTicks(4622));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 8, 14, 6, 16, 92, DateTimeKind.Local).AddTicks(9567),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 9, 20, 11, 8, 133, DateTimeKind.Local).AddTicks(1932));

            migrationBuilder.AddForeignKey(
                name: "FK_QuaTrinhBoiDuong_CanBo",
                schema: "NS",
                table: "QuaTrinhBoiDuong",
                column: "IDCanBo",
                principalSchema: "NS",
                principalTable: "CanBo",
                principalColumn: "IDCanBo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuaTrinhBoiDuong_HinhThucDaoTao",
                schema: "NS",
                table: "QuaTrinhBoiDuong",
                column: "MaHinhThucDaoTao",
                principalSchema: "tMasterData",
                principalTable: "HinhThucDaoTao",
                principalColumn: "MaHinhThucDaoTao",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuaTrinhDaoTao_ChuyenNganh",
                schema: "NS",
                table: "QuaTrinhDaoTao",
                column: "MaChuyenNganh",
                principalSchema: "tMasterData",
                principalTable: "ChuyenNganh",
                principalColumn: "MaChuyenNganh",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuaTrinhDaoTao_HinhThucDaoTao",
                schema: "NS",
                table: "QuaTrinhDaoTao",
                column: "MaHinhThucDaoTao",
                principalSchema: "tMasterData",
                principalTable: "HinhThucDaoTao",
                principalColumn: "MaHinhThucDaoTao",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuaTrinhDaoTao_LoaiBangCap",
                schema: "NS",
                table: "QuaTrinhDaoTao",
                column: "MaLoaiBangCap",
                principalSchema: "tMasterData",
                principalTable: "LoaiBangCap",
                principalColumn: "MaLoaiBangCap",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
