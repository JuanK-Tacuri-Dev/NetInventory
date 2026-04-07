using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetInventory.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbAuditConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Method = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    UrlPattern = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbAuditConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbAuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CorrelationId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Method = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Path = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    QueryString = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    RequestBody = table.Column<string>(type: "TEXT", maxLength: 8000, nullable: true),
                    ResponseBody = table.Column<string>(type: "TEXT", maxLength: 8000, nullable: true),
                    StatusCode = table.Column<int>(type: "INTEGER", nullable: false),
                    DurationMs = table.Column<long>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    UserEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    OccurredAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbAuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbErrorLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReferenceCode = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CorrelationId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ExceptionType = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    StackTrace = table.Column<string>(type: "TEXT", maxLength: 8000, nullable: true),
                    Path = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Method = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbErrorLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbGeneralTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbGeneralTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SKU = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CategoryTableId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryCode = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    QuantityInStock = table.Column<int>(type: "INTEGER", nullable: false),
                    MinStock = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    MaxStock = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    OwnerId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbRefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsRevoked = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbRefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbGeneralValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TableId = table.Column<int>(type: "INTEGER", nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbGeneralValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbGeneralValues_tbGeneralTable_TableId",
                        column: x => x.TableId,
                        principalTable: "tbGeneralTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbGeneralValues_tbGeneralValues_ParentId",
                        column: x => x.ParentId,
                        principalTable: "tbGeneralValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbStockMovements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Reason = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbStockMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbStockMovements_tbProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tbProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbRoleClaims_tbRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tbRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbUserClaims_tbUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "tbUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_tbUserLogins_tbUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "tbUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_tbUserRoles_tbRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tbRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbUserRoles_tbUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "tbUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_tbUserTokens_tbUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "tbUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbAuditLogs_CorrelationId",
                table: "tbAuditLogs",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_tbAuditLogs_OccurredAt",
                table: "tbAuditLogs",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "IX_tbErrorLogs_CorrelationId",
                table: "tbErrorLogs",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_tbErrorLogs_OccurredAt",
                table: "tbErrorLogs",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "IX_tbErrorLogs_ReferenceCode",
                table: "tbErrorLogs",
                column: "ReferenceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbGeneralValues_ParentId",
                table: "tbGeneralValues",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_tbGeneralValues_TableId_Code",
                table: "tbGeneralValues",
                columns: new[] { "TableId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbProducts_OwnerId",
                table: "tbProducts",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_tbRefreshTokens_Token",
                table: "tbRefreshTokens",
                column: "Token");

            migrationBuilder.CreateIndex(
                name: "IX_tbRefreshTokens_UserId",
                table: "tbRefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tbRoleClaims_RoleId",
                table: "tbRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "tbRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbStockMovements_ProductId",
                table: "tbStockMovements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_tbUserClaims_UserId",
                table: "tbUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tbUserLogins_UserId",
                table: "tbUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tbUserRoles_RoleId",
                table: "tbUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "tbUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "tbUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.Sql(@"
                CREATE VIEW vw_Products AS
                SELECT p.Id, p.Name, p.SKU, p.CategoryTableId, p.CategoryCode,
                       COALESCE(gv.Description, '') AS CategoryDescription,
                       p.QuantityInStock, p.MinStock, p.MaxStock,
                       p.UnitPrice, p.CreatedAt, p.OwnerId
                FROM tbProducts p
                LEFT JOIN tbGeneralValues gv
                    ON gv.TableId = p.CategoryTableId AND gv.Code = p.CategoryCode
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_Products");

            migrationBuilder.DropTable(
                name: "tbAuditConfigs");

            migrationBuilder.DropTable(
                name: "tbAuditLogs");

            migrationBuilder.DropTable(
                name: "tbErrorLogs");

            migrationBuilder.DropTable(
                name: "tbGeneralValues");

            migrationBuilder.DropTable(
                name: "tbRefreshTokens");

            migrationBuilder.DropTable(
                name: "tbRoleClaims");

            migrationBuilder.DropTable(
                name: "tbStockMovements");

            migrationBuilder.DropTable(
                name: "tbUserClaims");

            migrationBuilder.DropTable(
                name: "tbUserLogins");

            migrationBuilder.DropTable(
                name: "tbUserRoles");

            migrationBuilder.DropTable(
                name: "tbUserTokens");

            migrationBuilder.DropTable(
                name: "tbGeneralTable");

            migrationBuilder.DropTable(
                name: "tbProducts");

            migrationBuilder.DropTable(
                name: "tbRoles");

            migrationBuilder.DropTable(
                name: "tbUsers");
        }
    }
}
