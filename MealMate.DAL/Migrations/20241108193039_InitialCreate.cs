using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MealMate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "application_user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FName = table.Column<string>(type: "text", nullable: false),
                    LName = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bill_promotion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplyPrice = table.Column<int>(type: "integer", nullable: false),
                    PromotionChance = table.Column<int>(type: "integer", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    StartDay = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDay = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_promotion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "customer_promotion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    StartDay = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDay = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_promotion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PName = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Weight = table.Column<int>(type: "integer", nullable: false),
                    ImageURL = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "product_category_promotion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    StartDay = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDay = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_category_promotion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "product_promotion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    StartDay = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDay = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_promotion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    OpeningDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ContactInfo = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_store", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_application_user_UserId",
                        column: x => x.UserId,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_application_user_UserId",
                        column: x => x.UserId,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_application_user_UserId",
                        column: x => x.UserId,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalMoneySpent = table.Column<decimal>(type: "numeric", nullable: false),
                    FortuneChance = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_customer_application_user_Id",
                        column: x => x.Id,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shipper",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleCapacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipper", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shipper_application_user_Id",
                        column: x => x.Id,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "store_manager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Salary = table.Column<double>(type: "double precision", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_store_manager", x => x.Id);
                    table.ForeignKey(
                        name: "FK_store_manager_application_user_Id",
                        column: x => x.Id,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_application_user_UserId",
                        column: x => x.UserId,
                        principalTable: "application_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "promote_category",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    PromotionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promote_category", x => new { x.ProductId, x.PromotionId });
                    table.ForeignKey(
                        name: "FK_promote_category_product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_promote_category_product_category_promotion_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "product_category_promotion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "promote_product",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    PromotionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promote_product", x => new { x.ProductId, x.PromotionId });
                    table.ForeignKey(
                        name: "FK_promote_product_product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_promote_product_product_promotion_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "product_promotion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "at",
                columns: table => new
                {
                    ProductID = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreID = table.Column<Guid>(type: "uuid", nullable: false),
                    NumberAtStore = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_at", x => new { x.ProductID, x.StoreID });
                    table.ForeignKey(
                        name: "FK_at_product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_at_store_StoreID",
                        column: x => x.StoreID,
                        principalTable: "store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "promote_customer",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PromotionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promote_customer", x => new { x.CustomerId, x.PromotionId });
                    table.ForeignKey(
                        name: "FK_promote_customer_customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_promote_customer_customer_promotion_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "customer_promotion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bill",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentMethod = table.Column<string>(type: "text", nullable: false),
                    DateAndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreID = table.Column<Guid>(type: "uuid", nullable: false),
                    ShipperID = table.Column<Guid>(type: "uuid", nullable: true),
                    TotalPrice = table.Column<double>(type: "double precision", nullable: false),
                    TotalWeight = table.Column<int>(type: "integer", nullable: false),
                    DeliveryStatus = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bill_customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bill_shipper_ShipperID",
                        column: x => x.ShipperID,
                        principalTable: "shipper",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_bill_store_StoreID",
                        column: x => x.StoreID,
                        principalTable: "store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "include",
                columns: table => new
                {
                    TransactionID = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductID = table.Column<Guid>(type: "uuid", nullable: false),
                    NumberOfProductInBill = table.Column<int>(type: "integer", nullable: false),
                    SubTotal = table.Column<double>(type: "double precision", precision: 3, scale: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_include", x => new { x.TransactionID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_include_bill_TransactionID",
                        column: x => x.TransactionID,
                        principalTable: "bill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_include_product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "promote_bill",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PromotionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promote_bill", x => new { x.TransactionId, x.PromotionId });
                    table.ForeignKey(
                        name: "FK_promote_bill_bill_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "bill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_promote_bill_bill_promotion_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "bill_promotion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "application_user",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "NormalizedEmailIndex",
                table: "application_user",
                column: "NormalizedEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "application_user",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_at_StoreID",
                table: "at",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_bill_CustomerID",
                table: "bill",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_bill_ShipperID",
                table: "bill",
                column: "ShipperID");

            migrationBuilder.CreateIndex(
                name: "IX_bill_StoreID",
                table: "bill",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_include_ProductID",
                table: "include",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_promote_bill_PromotionId",
                table: "promote_bill",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_promote_bill_TransactionId",
                table: "promote_bill",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_promote_category_PromotionId",
                table: "promote_category",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_promote_customer_PromotionId",
                table: "promote_customer",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_promote_product_PromotionId",
                table: "promote_product",
                column: "PromotionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "at");

            migrationBuilder.DropTable(
                name: "include");

            migrationBuilder.DropTable(
                name: "promote_bill");

            migrationBuilder.DropTable(
                name: "promote_category");

            migrationBuilder.DropTable(
                name: "promote_customer");

            migrationBuilder.DropTable(
                name: "promote_product");

            migrationBuilder.DropTable(
                name: "store_manager");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "bill");

            migrationBuilder.DropTable(
                name: "bill_promotion");

            migrationBuilder.DropTable(
                name: "product_category_promotion");

            migrationBuilder.DropTable(
                name: "customer_promotion");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "product_promotion");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "shipper");

            migrationBuilder.DropTable(
                name: "store");

            migrationBuilder.DropTable(
                name: "application_user");
        }
    }
}
