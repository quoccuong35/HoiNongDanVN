using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addCauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 7, 19, 32, 50, 194, DateTimeKind.Local).AddTicks(9154),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 7, 7, 46, 59, 816, DateTimeKind.Local).AddTicks(3331));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 7, 19, 32, 50, 194, DateTimeKind.Local).AddTicks(4870),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 7, 7, 46, 59, 816, DateTimeKind.Local).AddTicks(722));

            migrationBuilder.CreateTable(
                name: "CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac",
                schema: "tMasterData",
                columns: table => new
                {
                    Id_CLB_DN_MH_HTX_THT = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac", x => x.Id_CLB_DN_MH_HTX_THT);
                });

            migrationBuilder.CreateTable(
                name: "CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien",
                schema: "HV",
                columns: table => new
                {
                    IDHoiVien = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id_CLB_DN_MH_HTX_THT = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien", x => new { x.Id_CLB_DN_MH_HTX_THT, x.IDHoiVien });
                    table.ForeignKey(
                        name: "FK_CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien",
                        column: x => x.Id_CLB_DN_MH_HTX_THT,
                        principalSchema: "tMasterData",
                        principalTable: "CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac",
                        principalColumn: "Id_CLB_DN_MH_HTX_THT",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien",
                        column: x => x.IDHoiVien,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien_IDHoiVien",
                schema: "HV",
                table: "CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien",
                column: "IDHoiVien");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac_HoiVien",
                schema: "HV");

            migrationBuilder.DropTable(
                name: "CauLacBo_DoiNhom_MoHinh_HopTacXa_ToHopTac",
                schema: "tMasterData");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 7, 7, 46, 59, 816, DateTimeKind.Local).AddTicks(3331),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 7, 19, 32, 50, 194, DateTimeKind.Local).AddTicks(9154));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 11, 7, 7, 46, 59, 816, DateTimeKind.Local).AddTicks(722),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 11, 7, 19, 32, 50, 194, DateTimeKind.Local).AddTicks(4870));
        }
    }
}
