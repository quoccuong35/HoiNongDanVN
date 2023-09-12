using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addDaoTaoBoiDuong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 9, 20, 56, 54, 30, DateTimeKind.Local).AddTicks(6171),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 9, 20, 24, 16, 428, DateTimeKind.Local).AddTicks(6539));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 9, 20, 56, 54, 30, DateTimeKind.Local).AddTicks(3949),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 9, 20, 24, 16, 428, DateTimeKind.Local).AddTicks(4017));

            migrationBuilder.CreateTable(
                name: "DaoTaoBoiDuong",
                schema: "NS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDCanBo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TuNgay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DenNgay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaHinhThucDaoTao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaLoaiBangCap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NoiDungDaoTao = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaoTaoBoiDuong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DaoTaoBoiDuong_CanBo",
                        column: x => x.IDCanBo,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DaoTaoBoiDuong_MaLoaiBangCap",
                        column: x => x.MaLoaiBangCap,
                        principalSchema: "tMasterData",
                        principalTable: "LoaiBangCap",
                        principalColumn: "MaLoaiBangCap");
                    table.ForeignKey(
                        name: "FK_DaoTaoBoiDuongc_HinhThucDaoTao",
                        column: x => x.MaHinhThucDaoTao,
                        principalSchema: "tMasterData",
                        principalTable: "HinhThucDaoTao",
                        principalColumn: "MaHinhThucDaoTao");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DaoTaoBoiDuong_IDCanBo",
                schema: "NS",
                table: "DaoTaoBoiDuong",
                column: "IDCanBo");

            migrationBuilder.CreateIndex(
                name: "IX_DaoTaoBoiDuong_MaHinhThucDaoTao",
                schema: "NS",
                table: "DaoTaoBoiDuong",
                column: "MaHinhThucDaoTao");

            migrationBuilder.CreateIndex(
                name: "IX_DaoTaoBoiDuong_MaLoaiBangCap",
                schema: "NS",
                table: "DaoTaoBoiDuong",
                column: "MaLoaiBangCap");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DaoTaoBoiDuong",
                schema: "NS");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 9, 20, 24, 16, 428, DateTimeKind.Local).AddTicks(6539),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 9, 20, 56, 54, 30, DateTimeKind.Local).AddTicks(6171));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 9, 9, 20, 24, 16, 428, DateTimeKind.Local).AddTicks(4017),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 9, 9, 20, 56, 54, 30, DateTimeKind.Local).AddTicks(3949));
        }
    }
}
