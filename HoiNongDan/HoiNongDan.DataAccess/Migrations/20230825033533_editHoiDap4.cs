using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editHoiDap4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoiVienHoiDap_CanBo",
                schema: "HV",
                table: "HoiVienHoiDap");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 25, 10, 35, 31, 896, DateTimeKind.Local).AddTicks(5712),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 24, 9, 29, 30, 586, DateTimeKind.Local).AddTicks(7133));

            migrationBuilder.AlterColumn<Guid>(
                name: "IDHoivien",
                schema: "HV",
                table: "HoiVienHoiDap",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "AcountID",
                schema: "HV",
                table: "HoiVienHoiDap",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 25, 10, 35, 31, 896, DateTimeKind.Local).AddTicks(421),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 24, 9, 29, 30, 586, DateTimeKind.Local).AddTicks(4386));

            migrationBuilder.CreateIndex(
                name: "IX_HoiVienHoiDap_AcountID",
                schema: "HV",
                table: "HoiVienHoiDap",
                column: "AcountID");

            migrationBuilder.AddForeignKey(
                name: "FK_HoiVienHoiDap_Account",
                schema: "HV",
                table: "HoiVienHoiDap",
                column: "AcountID",
                principalSchema: "pms",
                principalTable: "Account",
                principalColumn: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoiVienHoiDap_CanBo",
                schema: "HV",
                table: "HoiVienHoiDap",
                column: "IDHoivien",
                principalSchema: "NS",
                principalTable: "CanBo",
                principalColumn: "IDCanBo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoiVienHoiDap_Account",
                schema: "HV",
                table: "HoiVienHoiDap");

            migrationBuilder.DropForeignKey(
                name: "FK_HoiVienHoiDap_CanBo",
                schema: "HV",
                table: "HoiVienHoiDap");

            migrationBuilder.DropIndex(
                name: "IX_HoiVienHoiDap_AcountID",
                schema: "HV",
                table: "HoiVienHoiDap");

            migrationBuilder.DropColumn(
                name: "AcountID",
                schema: "HV",
                table: "HoiVienHoiDap");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 24, 9, 29, 30, 586, DateTimeKind.Local).AddTicks(7133),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 25, 10, 35, 31, 896, DateTimeKind.Local).AddTicks(5712));

            migrationBuilder.AlterColumn<Guid>(
                name: "IDHoivien",
                schema: "HV",
                table: "HoiVienHoiDap",
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
                defaultValue: new DateTime(2023, 8, 24, 9, 29, 30, 586, DateTimeKind.Local).AddTicks(4386),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 25, 10, 35, 31, 896, DateTimeKind.Local).AddTicks(421));

            migrationBuilder.AddForeignKey(
                name: "FK_HoiVienHoiDap_CanBo",
                schema: "HV",
                table: "HoiVienHoiDap",
                column: "IDHoivien",
                principalSchema: "NS",
                principalTable: "CanBo",
                principalColumn: "IDCanBo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
