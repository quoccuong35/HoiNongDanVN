using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editDepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 8, 20, 0, 16, 167, DateTimeKind.Local).AddTicks(3068),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 7, 19, 58, 58, 581, DateTimeKind.Local).AddTicks(570));

            migrationBuilder.AddColumn<Guid>(
                name: "IDCoSo",
                schema: "tMasterData",
                table: "Department",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 8, 20, 0, 16, 166, DateTimeKind.Local).AddTicks(8226),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 7, 19, 58, 58, 580, DateTimeKind.Local).AddTicks(6240));

            migrationBuilder.CreateIndex(
                name: "IX_Department_IDCoSo",
                schema: "tMasterData",
                table: "Department",
                column: "IDCoSo");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_CoSo",
                schema: "tMasterData",
                table: "Department",
                column: "IDCoSo",
                principalSchema: "tMasterData",
                principalTable: "CoSoModel",
                principalColumn: "IdCoSo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_CoSo",
                schema: "tMasterData",
                table: "Department");

            migrationBuilder.DropIndex(
                name: "IX_Department_IDCoSo",
                schema: "tMasterData",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "IDCoSo",
                schema: "tMasterData",
                table: "Department");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 7, 19, 58, 58, 581, DateTimeKind.Local).AddTicks(570),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 8, 20, 0, 16, 167, DateTimeKind.Local).AddTicks(3068));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 7, 19, 58, 58, 580, DateTimeKind.Local).AddTicks(6240),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 8, 20, 0, 16, 166, DateTimeKind.Local).AddTicks(8226));
        }
    }
}
