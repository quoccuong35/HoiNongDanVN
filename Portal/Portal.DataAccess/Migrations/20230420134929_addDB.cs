using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.DataAccess.Migrations
{
    public partial class addDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "pms");

            migrationBuilder.EnsureSchema(
                name: "tMasterData");

            migrationBuilder.EnsureSchema(
                name: "utilities");

            migrationBuilder.EnsureSchema(
                name: "entity");

            migrationBuilder.CreateTable(
                name: "Account",
                schema: "pms",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Bank",
                schema: "tMasterData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoSoModel",
                schema: "tMasterData",
                columns: table => new
                {
                    IdCoSo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenCoSo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValue: new DateTime(2023, 4, 20, 20, 49, 29, 91, DateTimeKind.Local).AddTicks(8473)),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoSoModel", x => x.IdCoSo);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                schema: "tMasterData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FunctionModel",
                schema: "pms",
                columns: table => new
                {
                    FunctionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FunctionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionModel", x => x.FunctionId);
                });

            migrationBuilder.CreateTable(
                name: "HistoryModel",
                schema: "utilities",
                columns: table => new
                {
                    HistoryModifyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OldData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryModel", x => x.HistoryModifyId);
                });

            migrationBuilder.CreateTable(
                name: "MenuModel",
                schema: "pms",
                columns: table => new
                {
                    MenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MenuName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Actived = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Href = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, defaultValue: "#"),
                    MenuIdParent = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MenuType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuModel", x => x.MenuId);
                    table.ForeignKey(
                        name: "FK_MenuModel_MenuModel_MenuIdParent",
                        column: x => x.MenuIdParent,
                        principalSchema: "pms",
                        principalTable: "MenuModel",
                        principalColumn: "MenuId");
                });

            migrationBuilder.CreateTable(
                name: "NgachLuongModel",
                schema: "tMasterData",
                columns: table => new
                {
                    MaNgachLuong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenNgachLuong = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NamTangLuong = table.Column<int>(type: "int", nullable: false),
                    MaLoai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValue: new DateTime(2023, 4, 20, 20, 49, 29, 92, DateTimeKind.Local).AddTicks(1855)),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NgachLuongModel", x => x.MaNgachLuong);
                });

            migrationBuilder.CreateTable(
                name: "ProjectModel",
                schema: "entity",
                columns: table => new
                {
                    ProjectID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValue: new DateTime(2023, 4, 20, 20, 49, 29, 92, DateTimeKind.Local).AddTicks(5492)),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectModel", x => x.ProjectID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolesModel",
                schema: "pms",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RolesCode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    RolesName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: true),
                    Actived = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesModel", x => x.RolesId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PageFunctionModel",
                schema: "pms",
                columns: table => new
                {
                    MenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FunctionId = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageFunctionModel", x => new { x.MenuId, x.FunctionId });
                    table.ForeignKey(
                        name: "FK_PageFunctionModel_FunctionModel",
                        column: x => x.FunctionId,
                        principalSchema: "pms",
                        principalTable: "FunctionModel",
                        principalColumn: "FunctionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PageFunctionModel_MenuModel",
                        column: x => x.MenuId,
                        principalSchema: "pms",
                        principalTable: "MenuModel",
                        principalColumn: "MenuId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BacLuongModel",
                schema: "tMasterData",
                columns: table => new
                {
                    MaBacLuong = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenBacLuong = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HeSo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Actived = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: true),
                    MaNgachLuong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BacLuongModel", x => x.MaBacLuong);
                    table.ForeignKey(
                        name: "FK_BacLuong_NgachLuong",
                        column: x => x.MaNgachLuong,
                        principalSchema: "tMasterData",
                        principalTable: "NgachLuongModel",
                        principalColumn: "MaNgachLuong",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountInRoleModel",
                schema: "pms",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountInRoleModel", x => new { x.AccountId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_AccountInRoleModel_Account",
                        column: x => x.AccountId,
                        principalSchema: "pms",
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_AccountInRoleModel_RolesModel",
                        column: x => x.RolesId,
                        principalSchema: "pms",
                        principalTable: "RolesModel",
                        principalColumn: "RolesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PagePermissionModel",
                schema: "pms",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FunctionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagePermissionModel", x => new { x.MenuId, x.FunctionId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_PagePermissionModel_Function",
                        column: x => x.FunctionId,
                        principalSchema: "pms",
                        principalTable: "FunctionModel",
                        principalColumn: "FunctionId");
                    table.ForeignKey(
                        name: "FK_PagePermissionModel_Menu",
                        column: x => x.MenuId,
                        principalSchema: "pms",
                        principalTable: "MenuModel",
                        principalColumn: "MenuId");
                    table.ForeignKey(
                        name: "FK_PagePermissionModel_Roles",
                        column: x => x.RolesId,
                        principalSchema: "pms",
                        principalTable: "RolesModel",
                        principalColumn: "RolesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountInRoleModel_RolesId",
                schema: "pms",
                table: "AccountInRoleModel",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_BacLuongModel_MaNgachLuong",
                schema: "tMasterData",
                table: "BacLuongModel",
                column: "MaNgachLuong");

            migrationBuilder.CreateIndex(
                name: "IX_MenuModel_MenuIdParent",
                schema: "pms",
                table: "MenuModel",
                column: "MenuIdParent");

            migrationBuilder.CreateIndex(
                name: "IX_PageFunctionModel_FunctionId",
                schema: "pms",
                table: "PageFunctionModel",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissionModel_FunctionId",
                schema: "pms",
                table: "PagePermissionModel",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissionModel_RolesId",
                schema: "pms",
                table: "PagePermissionModel",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountInRoleModel",
                schema: "pms");

            migrationBuilder.DropTable(
                name: "BacLuongModel",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "Bank",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "CoSoModel",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "Department",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "HistoryModel",
                schema: "utilities");

            migrationBuilder.DropTable(
                name: "PageFunctionModel",
                schema: "pms");

            migrationBuilder.DropTable(
                name: "PagePermissionModel",
                schema: "pms");

            migrationBuilder.DropTable(
                name: "ProjectModel",
                schema: "entity");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Account",
                schema: "pms");

            migrationBuilder.DropTable(
                name: "NgachLuongModel",
                schema: "tMasterData");

            migrationBuilder.DropTable(
                name: "FunctionModel",
                schema: "pms");

            migrationBuilder.DropTable(
                name: "MenuModel",
                schema: "pms");

            migrationBuilder.DropTable(
                name: "RolesModel",
                schema: "pms");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
