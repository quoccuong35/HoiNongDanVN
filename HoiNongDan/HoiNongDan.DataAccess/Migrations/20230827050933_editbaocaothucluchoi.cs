using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class editbaocaothucluchoi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThucLucHois");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 27, 12, 9, 31, 959, DateTimeKind.Local).AddTicks(2547),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 26, 17, 4, 37, 846, DateTimeKind.Local).AddTicks(8552));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 27, 12, 9, 31, 959, DateTimeKind.Local).AddTicks(102),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 26, 17, 4, 37, 846, DateTimeKind.Local).AddTicks(5902));

            migrationBuilder.CreateTable(
                name: "BaoCaoThucLucHoi",
                schema: "HV",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDDonVi = table.Column<int>(type: "int", nullable: false),
                    Cot1 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot2 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot3 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot4 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot5 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot6 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot7 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot8 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot9 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot10 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot11 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot12 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot13 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot14 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot15 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot16 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot17 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot18 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot19 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot20 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot21 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot22 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot23 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot24 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot25 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot26 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot27 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot28 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot29 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot30 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot31 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot32 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot33 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot34 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot35 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot36 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot37 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot38 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot39 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot40 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot41 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot42 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot43 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot44 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot45 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot46 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot47 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot48 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot49 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot50 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot51 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot52 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot53 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot54 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot55 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot56 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot57 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot58 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot59 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot60 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot61 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot62 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot63 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot64 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot65 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot66 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot67 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot68 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot69 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot70 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot71 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot72 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot73 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot74 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot75 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot76 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot77 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot78 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot79 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cot80 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Loai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nam_Quy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaoCaoThucLucHoi", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaoCaoThucLucHoi",
                schema: "HV");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 26, 17, 4, 37, 846, DateTimeKind.Local).AddTicks(8552),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 27, 12, 9, 31, 959, DateTimeKind.Local).AddTicks(2547));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 26, 17, 4, 37, 846, DateTimeKind.Local).AddTicks(5902),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 27, 12, 9, 31, 959, DateTimeKind.Local).AddTicks(102));

            migrationBuilder.CreateTable(
                name: "ThucLucHois",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Giam_HV_Nu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Giam_HV_TongSo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HN_PhatTrien_Nu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HN_PhatTrien_Tong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_DanhDu_LuyKe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_DanhDu_Nam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_Giam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_LuyKe_Giam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_LuyKe_Tang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_LuyKe_TrongKy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_NN_Giam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_NN_LuyKe_Giam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_NN_LuyKe_Tang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_NN_LuyKe_TrongKy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_NN_PhatTrien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_Nu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_PhatTrien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HV_TongSo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenDV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TongAp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TongKhuVuc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tong_HNN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tong_HNN_Giam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tong_HNN_HV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tong_HNN_HV_Giam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tong_LD_NN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tong_LD_NN_Giam = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThucLucHois", x => x.Id);
                });
        }
    }
}
