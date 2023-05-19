using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.DataAccess.Migrations
{
    public partial class EditDinhKem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileDinhKem",
                schema: "NS",
                table: "QuaTrinhBoNhiem");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 17, 20, 2, 6, 126, DateTimeKind.Local).AddTicks(8960),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 17, 19, 24, 57, 122, DateTimeKind.Local).AddTicks(4238));

            migrationBuilder.AddColumn<Guid>(
                name: "IdCanBo",
                schema: "tMasterData",
                table: "FileDinhKem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 17, 20, 2, 6, 126, DateTimeKind.Local).AddTicks(4800),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 17, 19, 24, 57, 122, DateTimeKind.Local).AddTicks(1344));

            migrationBuilder.CreateIndex(
                name: "IX_FileDinhKem_IdCanBo",
                schema: "tMasterData",
                table: "FileDinhKem",
                column: "IdCanBo");

            migrationBuilder.AddForeignKey(
                name: "FK_FileDinhKem_CanBo",
                schema: "tMasterData",
                table: "FileDinhKem",
                column: "IdCanBo",
                principalSchema: "NS",
                principalTable: "CanBo",
                principalColumn: "IDCanBo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileDinhKem_CanBo",
                schema: "tMasterData",
                table: "FileDinhKem");

            migrationBuilder.DropIndex(
                name: "IX_FileDinhKem_IdCanBo",
                schema: "tMasterData",
                table: "FileDinhKem");

            migrationBuilder.DropColumn(
                name: "IdCanBo",
                schema: "tMasterData",
                table: "FileDinhKem");

            migrationBuilder.AddColumn<string>(
                name: "FileDinhKem",
                schema: "NS",
                table: "QuaTrinhBoNhiem",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 17, 19, 24, 57, 122, DateTimeKind.Local).AddTicks(4238),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 17, 20, 2, 6, 126, DateTimeKind.Local).AddTicks(8960));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 17, 19, 24, 57, 122, DateTimeKind.Local).AddTicks(1344),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 17, 20, 2, 6, 126, DateTimeKind.Local).AddTicks(4800));
        }
    }
}
