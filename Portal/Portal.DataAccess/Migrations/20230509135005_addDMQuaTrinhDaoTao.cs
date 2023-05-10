using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.DataAccess.Migrations
{
    public partial class addDMQuaTrinhDaoTao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                oldDefaultValue: new DateTime(2023, 5, 8, 20, 0, 16, 167, DateTimeKind.Local).AddTicks(3068));

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
                oldDefaultValue: new DateTime(2023, 5, 8, 20, 0, 16, 166, DateTimeKind.Local).AddTicks(8226));

            migrationBuilder.CreateTable(
                name: "ChuyenNganh",
                schema: "tMasterData",
                columns: table => new
                {
                    MaChuyenNganh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenChuyenNganh = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChuyenNganh", x => x.MaChuyenNganh);
                });

            migrationBuilder.CreateTable(
                name: "HinhThucDaoTao",
                schema: "tMasterData",
                columns: table => new
                {
                    MaHinhThucDaoTao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenHinhThucDaoTao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhThucDaoTao", x => x.MaHinhThucDaoTao);
                });

            migrationBuilder.CreateTable(
                name: "LoaiBangCap",
                schema: "tMasterData",
                columns: table => new
                {
                    MaLoaiBangCap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenLoaiBangCap = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiBangCap", x => x.MaLoaiBangCap);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChuyenNganh",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "HinhThucDaoTao",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "LoaiBangCap",
                schema: "tMasterData");

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
                oldDefaultValue: new DateTime(2023, 5, 9, 20, 50, 4, 797, DateTimeKind.Local).AddTicks(5452));

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
                oldDefaultValue: new DateTime(2023, 5, 9, 20, 50, 4, 797, DateTimeKind.Local).AddTicks(974));
        }
    }
}
