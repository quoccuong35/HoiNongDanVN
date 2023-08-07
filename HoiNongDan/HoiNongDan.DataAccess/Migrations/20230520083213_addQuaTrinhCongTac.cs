using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addQuaTrinhCongTac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                oldDefaultValue: new DateTime(2023, 5, 18, 9, 11, 25, 423, DateTimeKind.Local).AddTicks(5061));

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
                oldDefaultValue: new DateTime(2023, 5, 18, 9, 11, 25, 422, DateTimeKind.Local).AddTicks(9507));

            migrationBuilder.CreateTable(
                name: "QuaTrinhCongTac",
                schema: "NS",
                columns: table => new
                {
                    IDQuaTrinhCongTac = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDCanBo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaChucVu = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoiLamViec = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuaTrinhCongTac", x => x.IDQuaTrinhCongTac);
                    table.ForeignKey(
                        name: "FK_QuaTrinhCongTac_CanBo",
                        column: x => x.IDCanBo,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuaTrinhCongTac_ChucVu",
                        column: x => x.MaChucVu,
                        principalSchema: "tMasterData",
                        principalTable: "ChucVuModel",
                        principalColumn: "MaChucVu");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhCongTac_IDCanBo",
                schema: "NS",
                table: "QuaTrinhCongTac",
                column: "IDCanBo");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhCongTac_MaChucVu",
                schema: "NS",
                table: "QuaTrinhCongTac",
                column: "MaChucVu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuaTrinhCongTac",
                schema: "NS");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 18, 9, 11, 25, 423, DateTimeKind.Local).AddTicks(5061),
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
                defaultValue: new DateTime(2023, 5, 18, 9, 11, 25, 422, DateTimeKind.Local).AddTicks(9507),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 20, 15, 32, 12, 158, DateTimeKind.Local).AddTicks(3518));
        }
    }
}
