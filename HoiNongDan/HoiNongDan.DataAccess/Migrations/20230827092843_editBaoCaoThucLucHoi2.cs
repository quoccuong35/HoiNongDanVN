using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editBaoCaoThucLucHoi2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nam_Quy",
                schema: "HV",
                table: "BaoCaoThucLucHoi",
                newName: "Nam");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 27, 16, 28, 41, 65, DateTimeKind.Local).AddTicks(7319),
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
                defaultValue: new DateTime(2023, 8, 27, 16, 28, 41, 65, DateTimeKind.Local).AddTicks(4323),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 27, 14, 22, 3, 861, DateTimeKind.Local).AddTicks(6471));

            migrationBuilder.AddColumn<int>(
                name: "Quy",
                schema: "HV",
                table: "BaoCaoThucLucHoi",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quy",
                schema: "HV",
                table: "BaoCaoThucLucHoi");

            migrationBuilder.RenameColumn(
                name: "Nam",
                schema: "HV",
                table: "BaoCaoThucLucHoi",
                newName: "Nam_Quy");

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
                oldDefaultValue: new DateTime(2023, 8, 27, 16, 28, 41, 65, DateTimeKind.Local).AddTicks(7319));

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
                oldDefaultValue: new DateTime(2023, 8, 27, 16, 28, 41, 65, DateTimeKind.Local).AddTicks(4323));
        }
    }
}
