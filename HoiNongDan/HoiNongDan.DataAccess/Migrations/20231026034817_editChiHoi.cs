using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editChiHoi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Actived",
                schema: "tMasterData",
                table: "ToHoi",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedAccountId",
                schema: "tMasterData",
                table: "ToHoi",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "ToHoi",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "tMasterData",
                table: "ToHoi",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedAccountId",
                schema: "tMasterData",
                table: "ToHoi",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedTime",
                schema: "tMasterData",
                table: "ToHoi",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                schema: "tMasterData",
                table: "ToHoi",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 10, 26, 10, 48, 16, 840, DateTimeKind.Local).AddTicks(9667),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 10, 19, 20, 17, 55, 651, DateTimeKind.Local).AddTicks(8437));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 10, 26, 10, 48, 16, 840, DateTimeKind.Local).AddTicks(6268),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 10, 19, 20, 17, 55, 651, DateTimeKind.Local).AddTicks(6357));

            migrationBuilder.AddColumn<bool>(
                name: "Actived",
                schema: "tMasterData",
                table: "ChiHoi",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedAccountId",
                schema: "tMasterData",
                table: "ChiHoi",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "ChiHoi",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "tMasterData",
                table: "ChiHoi",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedAccountId",
                schema: "tMasterData",
                table: "ChiHoi",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedTime",
                schema: "tMasterData",
                table: "ChiHoi",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                schema: "tMasterData",
                table: "ChiHoi",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actived",
                schema: "tMasterData",
                table: "ToHoi");

            migrationBuilder.DropColumn(
                name: "CreatedAccountId",
                schema: "tMasterData",
                table: "ToHoi");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "ToHoi");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "tMasterData",
                table: "ToHoi");

            migrationBuilder.DropColumn(
                name: "LastModifiedAccountId",
                schema: "tMasterData",
                table: "ToHoi");

            migrationBuilder.DropColumn(
                name: "LastModifiedTime",
                schema: "tMasterData",
                table: "ToHoi");

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                schema: "tMasterData",
                table: "ToHoi");

            migrationBuilder.DropColumn(
                name: "Actived",
                schema: "tMasterData",
                table: "ChiHoi");

            migrationBuilder.DropColumn(
                name: "CreatedAccountId",
                schema: "tMasterData",
                table: "ChiHoi");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "ChiHoi");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "tMasterData",
                table: "ChiHoi");

            migrationBuilder.DropColumn(
                name: "LastModifiedAccountId",
                schema: "tMasterData",
                table: "ChiHoi");

            migrationBuilder.DropColumn(
                name: "LastModifiedTime",
                schema: "tMasterData",
                table: "ChiHoi");

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                schema: "tMasterData",
                table: "ChiHoi");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 10, 19, 20, 17, 55, 651, DateTimeKind.Local).AddTicks(8437),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 10, 26, 10, 48, 16, 840, DateTimeKind.Local).AddTicks(9667));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 10, 19, 20, 17, 55, 651, DateTimeKind.Local).AddTicks(6357),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 10, 26, 10, 48, 16, 840, DateTimeKind.Local).AddTicks(6268));
        }
    }
}
