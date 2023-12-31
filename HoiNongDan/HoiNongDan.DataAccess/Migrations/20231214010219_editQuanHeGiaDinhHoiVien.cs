using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editQuanHeGiaDinhHoiVien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 12, 14, 8, 2, 18, 657, DateTimeKind.Local).AddTicks(4457),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 29, 15, 13, 3, 653, DateTimeKind.Local).AddTicks(4729));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 12, 14, 8, 2, 18, 657, DateTimeKind.Local).AddTicks(150),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 29, 15, 13, 3, 653, DateTimeKind.Local).AddTicks(2076));

            migrationBuilder.CreateIndex(
                name: "IX_QuanHeGiaDinh_IDHoiVien",
                schema: "NS",
                table: "QuanHeGiaDinh",
                column: "IDHoiVien");

            migrationBuilder.AddForeignKey(
                name: "FK_QuanHeGiaDinh_HoiVien",
                schema: "NS",
                table: "QuanHeGiaDinh",
                column: "IDHoiVien",
                principalSchema: "NS",
                principalTable: "CanBo",
                principalColumn: "IDCanBo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuanHeGiaDinh_HoiVien",
                schema: "NS",
                table: "QuanHeGiaDinh");

            migrationBuilder.DropIndex(
                name: "IX_QuanHeGiaDinh_IDHoiVien",
                schema: "NS",
                table: "QuanHeGiaDinh");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 29, 15, 13, 3, 653, DateTimeKind.Local).AddTicks(4729),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 12, 14, 8, 2, 18, 657, DateTimeKind.Local).AddTicks(4457));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 29, 15, 13, 3, 653, DateTimeKind.Local).AddTicks(2076),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 12, 14, 8, 2, 18, 657, DateTimeKind.Local).AddTicks(150));
        }
    }
}
