using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editCauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 4, 3, 21, 11, 41, 829, DateTimeKind.Local).AddTicks(1675),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 29, 8, 20, 21, 439, DateTimeKind.Local).AddTicks(6095));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 4, 3, 21, 11, 41, 828, DateTimeKind.Local).AddTicks(9198),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 3, 29, 8, 20, 21, 439, DateTimeKind.Local).AddTicks(3354));

            migrationBuilder.AddColumn<string>(
                name: "Loai",
                schema: "tMasterData",
                table: "CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Loai",
                schema: "tMasterData",
                table: "CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 29, 8, 20, 21, 439, DateTimeKind.Local).AddTicks(6095),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 4, 3, 21, 11, 41, 829, DateTimeKind.Local).AddTicks(1675));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 3, 29, 8, 20, 21, 439, DateTimeKind.Local).AddTicks(3354),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 4, 3, 21, 11, 41, 828, DateTimeKind.Local).AddTicks(9198));
        }
    }
}
