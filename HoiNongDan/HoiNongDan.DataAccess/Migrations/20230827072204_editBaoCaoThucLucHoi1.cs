using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editBaoCaoThucLucHoi1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 27, 14, 22, 3, 861, DateTimeKind.Local).AddTicks(9026),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 27, 13, 19, 44, 431, DateTimeKind.Local).AddTicks(1392));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 27, 14, 22, 3, 861, DateTimeKind.Local).AddTicks(6471),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 27, 13, 19, 44, 430, DateTimeKind.Local).AddTicks(6062));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedAccountId",
                schema: "HV",
                table: "BaoCaoThucLucHoi",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                schema: "HV",
                table: "BaoCaoThucLucHoi",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAccountId",
                schema: "HV",
                table: "BaoCaoThucLucHoi");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                schema: "HV",
                table: "BaoCaoThucLucHoi");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 27, 13, 19, 44, 431, DateTimeKind.Local).AddTicks(1392),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 27, 14, 22, 3, 861, DateTimeKind.Local).AddTicks(9026));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 27, 13, 19, 44, 430, DateTimeKind.Local).AddTicks(6062),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 27, 14, 22, 3, 861, DateTimeKind.Local).AddTicks(6471));
        }
    }
}
