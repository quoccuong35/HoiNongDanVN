using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HoiNongDan.DataAccess.Migrations
{
    public partial class canBoQuaTrinhLuong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 21, 19, 1, 4, 659, DateTimeKind.Local).AddTicks(8838),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 19, 19, 37, 5, 447, DateTimeKind.Local).AddTicks(5549));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 21, 19, 1, 4, 659, DateTimeKind.Local).AddTicks(6665),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 19, 19, 37, 5, 447, DateTimeKind.Local).AddTicks(2900));

            migrationBuilder.CreateTable(
                name: "CanBoQuaTrinhLuong",
                schema: "NS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDCanBo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaNgachLuong = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BacLuong = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HeoSoLuong = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HeSoChucVu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VuotKhung = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    KiemNhiem = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NgayHuong = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanBoQuaTrinhLuong", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CanBoQuaTrinhLuong_CanBo",
                        column: x => x.IDCanBo,
                        principalSchema: "NS",
                        principalTable: "CanBo",
                        principalColumn: "IDCanBo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CanBoQuaTrinhLuong_IDCanBo",
                schema: "NS",
                table: "CanBoQuaTrinhLuong",
                column: "IDCanBo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CanBoQuaTrinhLuong",
                schema: "NS");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "NgachLuongModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 19, 19, 37, 5, 447, DateTimeKind.Local).AddTicks(5549),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 21, 19, 1, 4, 659, DateTimeKind.Local).AddTicks(8838));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedTime",
                schema: "tMasterData",
                table: "CoSoModel",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 8, 19, 19, 37, 5, 447, DateTimeKind.Local).AddTicks(2900),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 8, 21, 19, 1, 4, 659, DateTimeKind.Local).AddTicks(6665));
        }
    }
}
