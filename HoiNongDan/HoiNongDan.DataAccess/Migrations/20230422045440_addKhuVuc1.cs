using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addKhuVuc1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_KhuVucs",
                table: "KhuVucs");

            migrationBuilder.RenameTable(
                name: "KhuVucs",
                newName: "KhuVucModel",
                newSchema: "tMasterData");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 22, 11, 54, 40, 190, DateTimeKind.Local).AddTicks(9075),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 22, 11, 48, 34, 212, DateTimeKind.Local).AddTicks(914));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 22, 11, 54, 40, 190, DateTimeKind.Local).AddTicks(2720),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 22, 11, 48, 34, 210, DateTimeKind.Local).AddTicks(7590));

            migrationBuilder.AddPrimaryKey(
                name: "PK_KhuVucModel",
                schema: "tMasterData",
                table: "KhuVucModel",
                column: "MaKhuVuc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_KhuVucModel",
                schema: "tMasterData",
                table: "KhuVucModel");

            migrationBuilder.RenameTable(
                name: "KhuVucModel",
                schema: "tMasterData",
                newName: "KhuVucs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 22, 11, 48, 34, 212, DateTimeKind.Local).AddTicks(914),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 22, 11, 54, 40, 190, DateTimeKind.Local).AddTicks(9075));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 22, 11, 48, 34, 210, DateTimeKind.Local).AddTicks(7590),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 22, 11, 54, 40, 190, DateTimeKind.Local).AddTicks(2720));

            migrationBuilder.AddPrimaryKey(
                name: "PK_KhuVucs",
                table: "KhuVucs",
                column: "MaKhuVuc");
        }
    }
}
