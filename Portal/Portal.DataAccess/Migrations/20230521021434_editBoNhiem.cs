using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.DataAccess.Migrations
{
    public partial class editBoNhiem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdCoSoCu",
                schema: "NS",
                table: "QuaTrinhBoNhiem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdDepartmentCu",
                schema: "NS",
                table: "QuaTrinhBoNhiem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Loai",
                schema: "NS",
                table: "QuaTrinhBoNhiem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "MaChucVuCu",
                schema: "NS",
                table: "QuaTrinhBoNhiem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 21, 9, 14, 33, 709, DateTimeKind.Local).AddTicks(5189),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 20, 15, 32, 12, 158, DateTimeKind.Local).AddTicks(8382));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 21, 9, 14, 33, 709, DateTimeKind.Local).AddTicks(304),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 20, 15, 32, 12, 158, DateTimeKind.Local).AddTicks(3518));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdCoSoCu",
                schema: "NS",
                table: "QuaTrinhBoNhiem");

            migrationBuilder.DropColumn(
                name: "IdDepartmentCu",
                schema: "NS",
                table: "QuaTrinhBoNhiem");

            migrationBuilder.DropColumn(
                name: "Loai",
                schema: "NS",
                table: "QuaTrinhBoNhiem");

            migrationBuilder.DropColumn(
                name: "MaChucVuCu",
                schema: "NS",
                table: "QuaTrinhBoNhiem");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 20, 15, 32, 12, 158, DateTimeKind.Local).AddTicks(8382),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 21, 9, 14, 33, 709, DateTimeKind.Local).AddTicks(5189));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 20, 15, 32, 12, 158, DateTimeKind.Local).AddTicks(3518),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 21, 9, 14, 33, 709, DateTimeKind.Local).AddTicks(304));
        }
    }
}
