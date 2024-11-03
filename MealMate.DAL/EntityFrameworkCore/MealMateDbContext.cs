using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.Entities.Products;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.Entities.Stores;
using MealMate.DAL.Entities.Transactions;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.EntityFrameworkCore
{
    public class MealMateDbContext(DbContextOptions<MealMateDbContext> options) : DbContext(options)
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<StoreManager> StoreManagers { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<BillPromotion> BillPromotions { get; set; }
        public DbSet<ProductPromotion> ProductPromotions { get; set; }
        public DbSet<ProductCategoryPromotion> ProductCategoryPromotions { get; set; }
        public DbSet<PromoteProduct> PromoteProducts { get; set; }
        public DbSet<PromoteCategory> PromoteCategories { get; set; }
        public DbSet<PromoteBill> PromoteBills { get; set; }
        public DbSet<AT> ATs { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Include> Includes { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "application_user", PortalConst.DbSchema);
            });

            builder.Entity<Customer>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "customer", PortalConst.DbSchema);
                b.HasBaseType<ApplicationUser>();
            });

            builder.Entity<StoreManager>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "store_manager", PortalConst.DbSchema);
                b.HasBaseType<ApplicationUser>();
            });

            builder.Entity<Shipper>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "shipper", PortalConst.DbSchema);
            });

            builder.Entity<BillPromotion>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "bill_promotion", PortalConst.DbSchema);
                b.HasBaseType<ApplicationUser>();
            });

            builder.Entity<ProductPromotion>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "product_promotion", PortalConst.DbSchema);
            });

            builder.Entity<ProductCategoryPromotion>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "product_category_promotion", PortalConst.DbSchema);
            });

            builder.Entity<PromoteProduct>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "promote_product", PortalConst.DbSchema);
            });

            builder.Entity<PromoteBill>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "promote_bill", PortalConst.DbSchema);
            });

            builder.Entity<PromoteCategory>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "promote_category", PortalConst.DbSchema);
            });

            builder.Entity<AT>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "at", PortalConst.DbSchema);
            });

            builder.Entity<Store>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "store", PortalConst.DbSchema);
            });

            builder.Entity<Bill>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "bill", PortalConst.DbSchema);
            });

            builder.Entity<Include>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "include", PortalConst.DbSchema);
            });

            builder.Entity<Product>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "product", PortalConst.DbSchema);
            });
        }
    }

}
