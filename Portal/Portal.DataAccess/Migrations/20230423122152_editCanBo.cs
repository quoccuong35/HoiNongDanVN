using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.DataAccess.Migrations
{
    public partial class editCanBo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoDem",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "NamSinh",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "NgaySinh",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "Ten",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "ThangSinh",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 23, 19, 21, 51, 576, DateTimeKind.Local).AddTicks(6093),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 23, 10, 8, 12, 158, DateTimeKind.Local).AddTicks(8456));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 23, 19, 21, 51, 576, DateTimeKind.Local).AddTicks(2219),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 23, 10, 8, 12, 158, DateTimeKind.Local).AddTicks(3977));

            migrationBuilder.AddColumn<string>(
                name: "HoVaTen",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "NgaySinh1",
                schema: "NS",
                table: "CanBo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoVaTen",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.DropColumn(
                name: "NgaySinh1",
                schema: "NS",
                table: "CanBo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 23, 10, 8, 12, 158, DateTimeKind.Local).AddTicks(8456),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 23, 19, 21, 51, 576, DateTimeKind.Local).AddTicks(6093));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 23, 10, 8, 12, 158, DateTimeKind.Local).AddTicks(3977),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 23, 19, 21, 51, 576, DateTimeKind.Local).AddTicks(2219));

            migrationBuilder.AddColumn<string>(
                name: "HoDem",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NamSinh",
                schema: "NS",
                table: "CanBo",
                type: "int",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NgaySinh",
                schema: "NS",
                table: "CanBo",
                type: "int",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ten",
                schema: "NS",
                table: "CanBo",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ThangSinh",
                schema: "NS",
                table: "CanBo",
                type: "int",
                maxLength: 2,
                nullable: true);
        }
    }
}
