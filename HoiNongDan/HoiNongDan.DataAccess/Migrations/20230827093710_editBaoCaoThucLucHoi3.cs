using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editBaoCaoThucLucHoi3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 27, 16, 37, 9, 518, DateTimeKind.Local).AddTicks(9585),
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
                defaultValue: new DateTime(2023, 8, 27, 16, 37, 9, 518, DateTimeKind.Local).AddTicks(6872),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 27, 16, 28, 41, 65, DateTimeKind.Local).AddTicks(4323));

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoThucLucHoi_IDDonVi",
                schema: "HV",
                table: "BaoCaoThucLucHoi",
                column: "IDDonVi");

            migrationBuilder.AddForeignKey(
                name: "FK_BaoCaoThucLucHoi_DonVi",
                schema: "HV",
                table: "BaoCaoThucLucHoi",
                column: "IDDonVi",
                principalSchema: "HV",
                principalTable: "DonVi",
                principalColumn: "IDDonVi",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaoCaoThucLucHoi_DonVi",
                schema: "HV",
                table: "BaoCaoThucLucHoi");

            migrationBuilder.DropIndex(
                name: "IX_BaoCaoThucLucHoi_IDDonVi",
                schema: "HV",
                table: "BaoCaoThucLucHoi");

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
                oldDefaultValue: new DateTime(2023, 8, 27, 16, 37, 9, 518, DateTimeKind.Local).AddTicks(9585));

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
                oldDefaultValue: new DateTime(2023, 8, 27, 16, 37, 9, 518, DateTimeKind.Local).AddTicks(6872));
        }
    }
}
