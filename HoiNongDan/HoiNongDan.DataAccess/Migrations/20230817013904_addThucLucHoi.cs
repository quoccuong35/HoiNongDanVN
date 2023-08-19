using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class addThucLucHoi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 17, 8, 39, 3, 178, DateTimeKind.Local).AddTicks(3021),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 16, 23, 46, 17, 828, DateTimeKind.Local).AddTicks(4549));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 17, 8, 39, 3, 178, DateTimeKind.Local).AddTicks(841),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 16, 23, 46, 17, 827, DateTimeKind.Local).AddTicks(9949));

            migrationBuilder.CreateTable(
                name: "ThucLucHois",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TongAp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TongKhuVuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tong_HNN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tong_HNN_Giam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tong_HNN_HV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tong_HNN_HV_Giam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tong_LD_NN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tong_LD_NN_Giam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_TongSo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_Nu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_PhatTrien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_Giam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_LuyKe_Tang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_LuyKe_Giam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_LuyKe_TrongKy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_NN_PhatTrien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_NN_Giam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_NN_LuyKe_Tang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_NN_LuyKe_Giam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_NN_LuyKe_TrongKy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_DanhDu_Nam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_DanhDu_LuyKe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Giam_HV_TongSo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Giam_HV_Nu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HN_PhatTrien_Tong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HN_PhatTrien_Nu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThucLucHois", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThucLucHois");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 16, 23, 46, 17, 828, DateTimeKind.Local).AddTicks(4549),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 17, 8, 39, 3, 178, DateTimeKind.Local).AddTicks(3021));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 16, 23, 46, 17, 827, DateTimeKind.Local).AddTicks(9949),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 17, 8, 39, 3, 178, DateTimeKind.Local).AddTicks(841));
        }
    }
}
