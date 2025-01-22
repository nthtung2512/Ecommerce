﻿// <auto-generated />
using System;
using MealMate.DAL.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MealMate.DAL.Migrations
{
    [DbContext(typeof(MealMateDbContext))]
    [Migration("20250121040905_InitialCreate3")]
    partial class InitialCreate3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MealMate.DAL.Entities.ApplicationUser.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("LName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedEmail")
                        .IsUnique()
                        .HasDatabaseName("NormalizedEmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("application_user", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Products.Bill", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerID")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateAndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DeliveryStatus")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ShipperID")
                        .HasColumnType("uuid");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("StoreID")
                        .HasColumnType("uuid");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("double precision");

                    b.Property<int>("TotalWeight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CustomerID");

                    b.HasIndex("ShipperID");

                    b.HasIndex("StoreID");

                    b.ToTable("bill", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Products.Include", b =>
                {
                    b.Property<Guid>("TransactionID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductID")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("NumberOfProductInBill")
                        .HasColumnType("integer");

                    b.Property<double>("SubTotal")
                        .HasPrecision(3, 2)
                        .HasColumnType("double precision");

                    b.HasKey("TransactionID", "ProductID");

                    b.HasIndex("ProductID");

                    b.ToTable("include", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Promotion.BillPromotion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("ApplyPrice")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Discount")
                        .HasPrecision(3, 2)
                        .HasColumnType("numeric(3,2)");

                    b.Property<DateTime>("EndDay")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PromotionChance")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDay")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("bill_promotion", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Promotion.CustomerPromotion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Discount")
                        .HasPrecision(3, 2)
                        .HasColumnType("numeric(3,2)");

                    b.Property<DateTime>("EndDay")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("StartDay")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("customer_promotion", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Promotion.ProductCategoryPromotion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Discount")
                        .HasPrecision(3, 2)
                        .HasColumnType("numeric(3,2)");

                    b.Property<DateTime>("EndDay")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDay")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("product_category_promotion", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Promotion.ProductPromotion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Discount")
                        .HasPrecision(3, 2)
                        .HasColumnType("numeric(3,2)");

                    b.Property<DateTime>("EndDay")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDay")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("product_promotion", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Promotion.PromoteBill", b =>
                {
                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PromotionId")
                        .HasColumnType("uuid");

                    b.HasKey("TransactionId", "PromotionId");

                    b.HasIndex("PromotionId");

                    b.HasIndex("TransactionId")
                        .IsUnique();

                    b.ToTable("promote_bill", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Promotion.PromoteCategory", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PromotionId")
                        .HasColumnType("uuid");

                    b.HasKey("ProductId", "PromotionId");

                    b.HasIndex("PromotionId");

                    b.ToTable("promote_category", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Promotion.PromoteCustomer", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PromotionId")
                        .HasColumnType("uuid");

                    b.HasKey("CustomerId", "PromotionId");

                    b.HasIndex("PromotionId");

                    b.ToTable("promote_customer", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Promotion.PromoteProduct", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PromotionId")
                        .HasColumnType("uuid");

                    b.HasKey("ProductId", "PromotionId");

                    b.HasIndex("PromotionId");

                    b.ToTable("promote_product", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Stores.AT", b =>
                {
                    b.Property<Guid>("ProductID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("StoreID")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("NumberAtStore")
                        .HasColumnType("integer");

                    b.HasKey("ProductID", "StoreID");

                    b.HasIndex("StoreID");

                    b.ToTable("at", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Stores.Store", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ContactInfo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Latitude")
                        .HasPrecision(10, 7)
                        .HasColumnType("numeric(10,7)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Longitude")
                        .HasPrecision(10, 7)
                        .HasColumnType("numeric(10,7)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("OpeningDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("store", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Transactions.CartItem", b =>
                {
                    b.Property<Guid>("CartItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductID")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<Guid>("StoreID")
                        .HasColumnType("uuid");

                    b.HasKey("CartItemID");

                    b.ToTable("cart_item", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Transactions.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("PName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<int>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("product", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.ApplicationUser.Customer", b =>
                {
                    b.HasBaseType("MealMate.DAL.Entities.ApplicationUser.ApplicationUser");

                    b.Property<int>("FortuneChance")
                        .HasColumnType("integer");

                    b.Property<decimal>("TotalMoneySpent")
                        .HasColumnType("numeric");

                    b.ToTable("customer", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.ApplicationUser.Shipper", b =>
                {
                    b.HasBaseType("MealMate.DAL.Entities.ApplicationUser.ApplicationUser");

                    b.Property<int>("VehicleCapacity")
                        .HasColumnType("integer");

                    b.ToTable("shipper", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.ApplicationUser.StoreManager", b =>
                {
                    b.HasBaseType("MealMate.DAL.Entities.ApplicationUser.ApplicationUser");

                    b.Property<double>("Salary")
                        .HasColumnType("double precision");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uuid");

                    b.ToTable("store_manager", (string)null);
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Products.Bill", b =>
                {
                    b.HasOne("MealMate.DAL.Entities.ApplicationUser.Customer", "Customer")
                        .WithMany("Bills")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MealMate.DAL.Entities.ApplicationUser.Shipper", "Shipper")
                        .WithMany("Bills")
                        .HasForeignKey("ShipperID");

                    b.HasOne("MealMate.DAL.Entities.Stores.Store", "Store")
                        .WithMany("Bills")
                        .HasForeignKey("StoreID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Shipper");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Products.Include", b =>
                {
                    b.HasOne("MealMate.DAL.Entities.Transactions.Product", "Product")
                        .WithMany("Includes")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MealMate.DAL.Entities.Products.Bill", "Transaction")
                        .WithMany("Includes")
                        .HasForeignKey("TransactionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Promotion.PromoteBill", b =>
                {
                    b.HasOne("MealMate.DAL.Entities.Promotion.BillPromotion", "BillPromotion")
                        .WithMany("PromoteBills")
                        .HasForeignKey("PromotionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MealMate.DAL.Entities.Products.Bill", "Bill")
                        .WithOne("PromoteBill")
                        .HasForeignKey("MealMate.DAL.Entities.Promotion.PromoteBill", "TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bill");

                    b.Navigation("BillPromotion");
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Promotion.PromoteCategory", b =>
                {
                    b.HasOne("MealMate.DAL.Entities.Transactions.Product", "Product")
                        .WithMany("PromoteCategories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MealMate.DAL.Entities.Promotion.ProductCategoryPromotion", "ProductCategoryPromotion")
                        .WithMany("PromoteProductCategories")
                        .HasForeignKey("PromotionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("ProductCategoryPromotion");
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Promotion.PromoteCustomer", b =>
                {
                    b.HasOne("MealMate.DAL.Entities.ApplicationUser.Customer", "Customer")
                        .WithMany("PromoteCustomers")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MealMate.DAL.Entities.Promotion.CustomerPromotion", "CustomerPromotion")
                        .WithMany("PromoteCustomers")
                        .HasForeignKey("PromotionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("CustomerPromotion");
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Promotion.PromoteProduct", b =>
                {
                    b.HasOne("MealMate.DAL.Entities.Transactions.Product", "Product")
                        .WithMany("PromoteProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MealMate.DAL.Entities.Promotion.ProductPromotion", "ProductPromotion")
                        .WithMany("PromoteProducts")
                        .HasForeignKey("PromotionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("ProductPromotion");
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Stores.AT", b =>
                {
                    b.HasOne("MealMate.DAL.Entities.Transactions.Product", "Product")
                        .WithMany("ATs")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MealMate.DAL.Entities.Stores.Store", "Store")
                        .WithMany("ATs")
                        .HasForeignKey("StoreID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("MealMate.DAL.Entities.ApplicationUser.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("MealMate.DAL.Entities.ApplicationUser.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MealMate.DAL.Entities.ApplicationUser.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("MealMate.DAL.Entities.ApplicationUser.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MealMate.DAL.Entities.ApplicationUser.Customer", b =>
                {
                    b.HasOne("MealMate.DAL.Entities.ApplicationUser.ApplicationUser", null)
                        .WithOne()
                        .HasForeignKey("MealMate.DAL.Entities.ApplicationUser.Customer", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MealMate.DAL.Entities.ApplicationUser.Shipper", b =>
                {
                    b.HasOne("MealMate.DAL.Entities.ApplicationUser.ApplicationUser", null)
                        .WithOne()
                        .HasForeignKey("MealMate.DAL.Entities.ApplicationUser.Shipper", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MealMate.DAL.Entities.ApplicationUser.StoreManager", b =>
                {
                    b.HasOne("MealMate.DAL.Entities.ApplicationUser.ApplicationUser", null)
                        .WithOne()
                        .HasForeignKey("MealMate.DAL.Entities.ApplicationUser.StoreManager", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Products.Bill", b =>
                {
                    b.Navigation("Includes");

                    b.Navigation("PromoteBill");
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Promotion.BillPromotion", b =>
                {
                    b.Navigation("PromoteBills");
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Promotion.CustomerPromotion", b =>
                {
                    b.Navigation("PromoteCustomers");
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Promotion.ProductCategoryPromotion", b =>
                {
                    b.Navigation("PromoteProductCategories");
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Promotion.ProductPromotion", b =>
                {
                    b.Navigation("PromoteProducts");
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Stores.Store", b =>
                {
                    b.Navigation("ATs");

                    b.Navigation("Bills");
                });

            modelBuilder.Entity("MealMate.DAL.Entities.Transactions.Product", b =>
                {
                    b.Navigation("ATs");

                    b.Navigation("Includes");

                    b.Navigation("PromoteCategories");

                    b.Navigation("PromoteProducts");
                });

            modelBuilder.Entity("MealMate.DAL.Entities.ApplicationUser.Customer", b =>
                {
                    b.Navigation("Bills");

                    b.Navigation("PromoteCustomers");
                });

            modelBuilder.Entity("MealMate.DAL.Entities.ApplicationUser.Shipper", b =>
                {
                    b.Navigation("Bills");
                });
#pragma warning restore 612, 618
        }
    }
}
