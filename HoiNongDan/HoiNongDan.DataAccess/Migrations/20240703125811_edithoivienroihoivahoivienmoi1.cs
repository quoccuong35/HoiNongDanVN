using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class edithoivienroihoivahoivienmoi1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 3, 19, 58, 10, 273, DateTimeKind.Local).AddTicks(9065),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 3, 19, 53, 4, 718, DateTimeKind.Local).AddTicks(602));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 3, 19, 58, 10, 273, DateTimeKind.Local).AddTicks(4313),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 3, 19, 53, 4, 717, DateTimeKind.Local).AddTicks(7714));

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountIdTuChoi",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "NgayTuChoi",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.RenameColumn(
                name: "TuChoi",
                schema: "NS",
                table: "CanBo",
                newName: "KhongDuyet");

            migrationBuilder.RenameColumn(
                name: "LyDoTuChoi",
                schema: "NS",
                table: "CanBo",
                newName: "LyDoKhongDuyet");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 3, 19, 53, 4, 718, DateTimeKind.Local).AddTicks(602),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 3, 19, 58, 10, 273, DateTimeKind.Local).AddTicks(9065));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 3, 19, 53, 4, 717, DateTimeKind.Local).AddTicks(7714),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 3, 19, 58, 10, 273, DateTimeKind.Local).AddTicks(4313));
        }
    }
}
