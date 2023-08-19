using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class EditDMChuyenNganh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrinhDoChuyenMon",
                schema: "tMasterData",
                table: "TrinhDoChuyenMon");

            migrationBuilder.DropColumn(
                name: "MaTrinhDoChuyenMon",
                schema: "tMasterData",
                table: "TrinhDoChuyenMon");

            migrationBuilder.AddColumn<string>(
                name: "MaTrinhDo",
                schema: "tMasterData",
                table: "TrinhDoChuyenMon",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 12, 15, 17, 5, 655, DateTimeKind.Local).AddTicks(3403),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 12, 15, 12, 49, 562, DateTimeKind.Local).AddTicks(4969));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 12, 15, 17, 5, 654, DateTimeKind.Local).AddTicks(9482),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 12, 15, 12, 49, 562, DateTimeKind.Local).AddTicks(310));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrinhDoChuyenMon",
                schema: "tMasterData",
                table: "TrinhDoChuyenMon",
                column: "MaTrinhDo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrinhDoChuyenMon",
                schema: "tMasterData",
                table: "TrinhDoChuyenMon");

            migrationBuilder.DropColumn(
                name: "MaTrinhDo",
                schema: "tMasterData",
                table: "TrinhDoChuyenMon");

            migrationBuilder.AddColumn<Guid>(
                name: "MaTrinhDoChuyenMon",
                schema: "tMasterData",
                table: "TrinhDoChuyenMon",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 12, 15, 12, 49, 562, DateTimeKind.Local).AddTicks(4969),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 12, 15, 17, 5, 655, DateTimeKind.Local).AddTicks(3403));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 12, 15, 12, 49, 562, DateTimeKind.Local).AddTicks(310),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 12, 15, 17, 5, 654, DateTimeKind.Local).AddTicks(9482));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrinhDoChuyenMon",
                schema: "tMasterData",
                table: "TrinhDoChuyenMon",
                column: "MaTrinhDoChuyenMon");
        }
    }
}
