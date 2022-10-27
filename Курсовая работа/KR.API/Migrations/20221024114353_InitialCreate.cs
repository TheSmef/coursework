using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KR.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id_Category = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id_Category);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id_Post = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id_Post);
                    table.CheckConstraint("CH_Salary_Post", "Salary > 0");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseAgreements",
                columns: table => new
                {
                    Id_Purchase_Agreement = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date_Of_Purchase = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Agreement_Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseAgreements", x => x.Id_Purchase_Agreement);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id_User = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    First_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Otch = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id_User);
                });

            migrationBuilder.CreateTable(
                name: "ProductStorages",
                columns: table => new
                {
                    Id_Product_Storage = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId_Category = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Exipiration_Date = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStorages", x => x.Id_Product_Storage);
                    table.CheckConstraint("CH_Amount_Product", "Amount >= 0");
                    table.CheckConstraint("CH_Cost_Product", "Cost > 0");
                    table.CheckConstraint("CH_Exrire_Product", "Exipiration_Date >= 0");
                    table.ForeignKey(
                        name: "FK_ProductStorages_Categories_CategoryId_Category",
                        column: x => x.CategoryId_Category,
                        principalTable: "Categories",
                        principalColumn: "Id_Category",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Login = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.UserId);
                    table.CheckConstraint("CH_Email", "Email like '%@%.%'");
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id_User",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPosts",
                columns: table => new
                {
                    Id_User_Post = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId_Post = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId_User = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Share = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPosts", x => x.Id_User_Post);
                    table.CheckConstraint("Ch_Share_UserPost", "Share > 0 AND Share <= 1");
                    table.ForeignKey(
                        name: "FK_UserPosts_Posts_PostId_Post",
                        column: x => x.PostId_Post,
                        principalTable: "Posts",
                        principalColumn: "Id_Post",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPosts_Users_UserId_User",
                        column: x => x.UserId_User,
                        principalTable: "Users",
                        principalColumn: "Id_User",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id_Puchase = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductStorageId_Product_Storage = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    PurchaseAgreementId_Purchase_Agreement = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
                    Date_Purchase = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_Creation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id_Puchase);
                    table.CheckConstraint("CH_Amount_Purchase", "Amount > 0");
                    table.CheckConstraint("CH_Price_Puchase", "Price > 0");
                    table.ForeignKey(
                        name: "FK_Purchases_ProductStorages_ProductStorageId_Product_Storage",
                        column: x => x.ProductStorageId_Product_Storage,
                        principalTable: "ProductStorages",
                        principalColumn: "Id_Product_Storage",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchases_PurchaseAgreements_PurchaseAgreementId_Purchase_Agreement",
                        column: x => x.PurchaseAgreementId_Purchase_Agreement,
                        principalTable: "PurchaseAgreements",
                        principalColumn: "Id_Purchase_Agreement",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StorageHistory",
                columns: table => new
                {
                    Id_StorageHistory = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductStorageId_Product_Storage = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageHistory", x => x.Id_StorageHistory);
                    table.ForeignKey(
                        name: "FK_StorageHistory_ProductStorages_ProductStorageId_Product_Storage",
                        column: x => x.ProductStorageId_Product_Storage,
                        principalTable: "ProductStorages",
                        principalColumn: "Id_Product_Storage",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id_Role = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AccountUserUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id_Role);
                    table.ForeignKey(
                        name: "FK_Roles_Accounts_AccountUserUserId",
                        column: x => x.AccountUserUserId,
                        principalTable: "Accounts",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id_Order = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserPostId_User_Post = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date_Order = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id_Order);
                    table.ForeignKey(
                        name: "FK_Orders_UserPosts_UserPostId_User_Post",
                        column: x => x.UserPostId_User_Post,
                        principalTable: "UserPosts",
                        principalColumn: "Id_User_Post",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalaryHistories",
                columns: table => new
                {
                    Id_SalaryHistory = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserPostId_User_Post = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Payment = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
                    Premium = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryHistories", x => x.Id_SalaryHistory);
                    table.CheckConstraint("CH_Payment_Salary", "Payment > 0");
                    table.ForeignKey(
                        name: "FK_SalaryHistories_UserPosts_UserPostId_User_Post",
                        column: x => x.UserPostId_User_Post,
                        principalTable: "UserPosts",
                        principalColumn: "Id_User_Post",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                columns: table => new
                {
                    Id_order_product = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId_Order = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId_Product_Storage = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducts", x => x.Id_order_product);
                    table.CheckConstraint("CH_Amount_Order", "Amount > 0");
                    table.CheckConstraint("CH_Price_Order", "Price > 0");
                    table.ForeignKey(
                        name: "FK_OrderProducts_Orders_OrderId_Order",
                        column: x => x.OrderId_Order,
                        principalTable: "Orders",
                        principalColumn: "Id_Order",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_ProductStorages_ProductId_Product_Storage",
                        column: x => x.ProductId_Product_Storage,
                        principalTable: "ProductStorages",
                        principalColumn: "Id_Product_Storage",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Email",
                table: "Accounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Login",
                table: "Accounts",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OrderId_Order",
                table: "OrderProducts",
                column: "OrderId_Order");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProductId_Product_Storage",
                table: "OrderProducts",
                column: "ProductId_Product_Storage");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserPostId_User_Post",
                table: "Orders",
                column: "UserPostId_User_Post");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Name",
                table: "Posts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductStorages_CategoryId_Category",
                table: "ProductStorages",
                column: "CategoryId_Category");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStorages_Name",
                table: "ProductStorages",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ProductStorageId_Product_Storage",
                table: "Purchases",
                column: "ProductStorageId_Product_Storage");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PurchaseAgreementId_Purchase_Agreement",
                table: "Purchases",
                column: "PurchaseAgreementId_Purchase_Agreement");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_AccountUserUserId",
                table: "Roles",
                column: "AccountUserUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryHistories_UserPostId_User_Post",
                table: "SalaryHistories",
                column: "UserPostId_User_Post");

            migrationBuilder.CreateIndex(
                name: "IX_StorageHistory_ProductStorageId_Product_Storage",
                table: "StorageHistory",
                column: "ProductStorageId_Product_Storage");

            migrationBuilder.CreateIndex(
                name: "IX_UserPosts_PostId_Post",
                table: "UserPosts",
                column: "PostId_Post");

            migrationBuilder.CreateIndex(
                name: "IX_UserPosts_UserId_User",
                table: "UserPosts",
                column: "UserId_User");

            using (StreamReader reader = new StreamReader("Migrations/SqlQuery.sql"))
            {
                migrationBuilder.Sql(reader.ReadToEnd());
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProducts");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "SalaryHistories");

            migrationBuilder.DropTable(
                name: "StorageHistory");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PurchaseAgreements");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "ProductStorages");

            migrationBuilder.DropTable(
                name: "UserPosts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Users");


            using (StreamReader reader = new StreamReader("Migrations/DROPQuery.sql"))
            {
                migrationBuilder.Sql(reader.ReadToEnd());
            }
        }
    }
}
