using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editCanBoAccout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 25, 19, 13, 15, 43, DateTimeKind.Local).AddTicks(3072),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 25, 16, 32, 59, 222, DateTimeKind.Local).AddTicks(6931));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 25, 19, 13, 15, 42, DateTimeKind.Local).AddTicks(9589),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 25, 16, 32, 59, 222, DateTimeKind.Local).AddTicks(2210));

            migrationBuilder.AddColumn<bool>(
                name: "Actived",
                schema: "NS",
                table: "CanBo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedAccountId",
                schema: "NS",
                table: "CanBo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                schema: "NS",
                table: "CanBo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedAccountId",
                schema: "NS",
                table: "CanBo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedTime",
                schema: "NS",
                table: "CanBo",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actived",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "CreatedAccountId",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "LastModifiedAccountId",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "LastModifiedTime",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 25, 16, 32, 59, 222, DateTimeKind.Local).AddTicks(6931),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 25, 19, 13, 15, 43, DateTimeKind.Local).AddTicks(3072));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 25, 16, 32, 59, 222, DateTimeKind.Local).AddTicks(2210),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 25, 19, 13, 15, 42, DateTimeKind.Local).AddTicks(9589));
        }
    }
}
