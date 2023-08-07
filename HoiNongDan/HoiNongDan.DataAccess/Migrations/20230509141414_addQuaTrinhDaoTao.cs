using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addQuaTrinhDaoTao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 9, 21, 14, 13, 221, DateTimeKind.Local).AddTicks(110),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 9, 20, 50, 4, 797, DateTimeKind.Local).AddTicks(5452));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 9, 21, 14, 13, 220, DateTimeKind.Local).AddTicks(4895),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 9, 20, 50, 4, 797, DateTimeKind.Local).AddTicks(974));

            migrationBuilder.CreateTable(
                name: "QuaTrinhDaoTao",
                schema: "NS",
                columns: table => new
                {
                    IDQuaTrinhDaoTao = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDCanBo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoSoDaoTao = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NgayTotNghiep = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    QuocGia = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    MaLoaiBangCap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaHinhThucDaoTao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaChuyenNganh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LuanAnTN = table.Column<bool>(type: "bit", nullable: true),
                    FileDinhKem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuaTrinhDaoTao", x => x.IDQuaTrinhDaoTao);
                    table.ForeignKey(
                        name: "FK_QuaTrinhDaoTao_CanBo",
                        column: x => x.IDCanBo,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuaTrinhDaoTao_ChuyenNganh",
                        column: x => x.MaChuyenNganh,
                        principalSchema: "tMasterData",
                        principalTable: "ChuyenNganh",
                        principalColumn: "MaChuyenNganh",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuaTrinhDaoTao_HinhThucDaoTao",
                        column: x => x.MaHinhThucDaoTao,
                        principalSchema: "tMasterData",
                        principalTable: "HinhThucDaoTao",
                        principalColumn: "MaHinhThucDaoTao",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuaTrinhDaoTao_LoaiBangCap",
                        column: x => x.MaLoaiBangCap,
                        principalSchema: "tMasterData",
                        principalTable: "LoaiBangCap",
                        principalColumn: "MaLoaiBangCap",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhDaoTao_IDCanBo",
                schema: "NS",
                table: "QuaTrinhDaoTao",
                column: "IDCanBo");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhDaoTao_MaChuyenNganh",
                schema: "NS",
                table: "QuaTrinhDaoTao",
                column: "MaChuyenNganh");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhDaoTao_MaHinhThucDaoTao",
                schema: "NS",
                table: "QuaTrinhDaoTao",
                column: "MaHinhThucDaoTao");

            migrationBuilder.CreateIndex(
                name: "IX_QuaTrinhDaoTao_MaLoaiBangCap",
                schema: "NS",
                table: "QuaTrinhDaoTao",
                column: "MaLoaiBangCap");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuaTrinhDaoTao",
                schema: "NS");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 9, 20, 50, 4, 797, DateTimeKind.Local).AddTicks(5452),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 9, 21, 14, 13, 221, DateTimeKind.Local).AddTicks(110));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 9, 20, 50, 4, 797, DateTimeKind.Local).AddTicks(974),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 9, 21, 14, 13, 220, DateTimeKind.Local).AddTicks(4895));
        }
    }
}
