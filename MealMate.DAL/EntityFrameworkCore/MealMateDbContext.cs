using MealMate.DAL.Entities.ApplicationUser;
using MealMate.DAL.Entities.Products;
using MealMate.DAL.Entities.Promotion;
using MealMate.DAL.Entities.Stores;
using MealMate.DAL.Entities.Transactions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MealMate.DAL.EntityFrameworkCore
{
    public class MealMateDbContext(DbContextOptions<MealMateDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<StoreManager> StoreManagers { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<BillPromotion> BillPromotions { get; set; }
        public DbSet<ProductPromotion> ProductPromotions { get; set; }
        public DbSet<ProductCategoryPromotion> ProductCategoryPromotions { get; set; }
        public DbSet<CustomerPromotion> CustomerPromotions { get; set; }
        public DbSet<PromoteProduct> PromoteProducts { get; set; }
        public DbSet<PromoteCategory> PromoteCategories { get; set; }
        public DbSet<PromoteBill> PromoteBills { get; set; }
        public DbSet<PromoteCustomer> PromoteCustomers { get; set; }
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

                // Create unique index for Email
                b.HasIndex(u => u.Email)
                    .HasDatabaseName("EmailIndex")
                    .IsUnique();

                // Create unique index for NormalizedEmail
                b.HasIndex(u => u.NormalizedEmail)
                    .HasDatabaseName("NormalizedEmailIndex")
                    .IsUnique();
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
                b.HasBaseType<ApplicationUser>();
            });

            builder.Entity<BillPromotion>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "bill_promotion", PortalConst.DbSchema);
            });

            builder.Entity<ProductPromotion>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "product_promotion", PortalConst.DbSchema);
            });

            builder.Entity<ProductCategoryPromotion>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "product_category_promotion", PortalConst.DbSchema);
            });

            builder.Entity<CustomerPromotion>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "customer_promotion", PortalConst.DbSchema);
            });

            builder.Entity<PromoteProduct>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "promote_product", PortalConst.DbSchema)
                 .HasKey(pp => new { pp.ProductId, pp.PromotionId });

                b.HasOne(pp => pp.Product)
                .WithMany(p => p.PromoteProducts)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(pp => pp.ProductPromotion)
                .WithMany(ppromotion => ppromotion.PromoteProducts)
                .HasForeignKey(pp => pp.PromotionId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<PromoteBill>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "promote_bill", PortalConst.DbSchema)
                .HasKey(pb => new { pb.TransactionId, pb.PromotionId });

                b.HasOne(pb => pb.Bill)
                .WithOne(b => b.PromoteBill)
                .HasForeignKey<PromoteBill>(pb => pb.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(pb => pb.BillPromotion)
                .WithMany(bp => bp.PromoteBills)
                .HasForeignKey(pp => pp.PromotionId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<PromoteCategory>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "promote_category", PortalConst.DbSchema)
                .HasKey(pc => new { pc.ProductId, pc.PromotionId });

                b.HasOne(pc => pc.Product)
                .WithMany(p => p.PromoteCategories)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(pc => pc.ProductCategoryPromotion)
                .WithMany(pcp => pcp.PromoteProductCategories)
                .HasForeignKey(pc => pc.PromotionId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<PromoteCustomer>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "promote_customer", PortalConst.DbSchema)
                .HasKey(pc => new { pc.CustomerId, pc.PromotionId });

                b.HasOne(pc => pc.Customer)
                .WithMany(c => c.PromoteCustomers)
                .HasForeignKey(pc => pc.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(pc => pc.CustomerPromotion)
                .WithMany(cp => cp.PromoteCustomers)
                .HasForeignKey(pc => pc.PromotionId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<AT>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "at", PortalConst.DbSchema)
                .HasKey(at => new { at.ProductID, at.StoreID });

                b.HasOne(at => at.Product)
                .WithMany(p => p.ATs)
                .HasForeignKey(at => at.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(at => at.Store)
                .WithMany(s => s.ATs)
                .HasForeignKey(at => at.StoreID)
                .OnDelete(DeleteBehavior.Cascade);
            });


            builder.Entity<Store>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "store", PortalConst.DbSchema);
            });

            builder.Entity<Bill>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "bill", PortalConst.DbSchema);

                b.HasOne(b => b.Customer)
                .WithMany(c => c.Bills)
                .HasForeignKey(b => b.CustomerID);

                b.HasOne(b => b.Store)
                .WithMany(s => s.Bills)
                .HasForeignKey(b => b.StoreID);

                b.HasOne(b => b.Shipper)
                .WithMany(s => s.Bills)
                .HasForeignKey(b => b.ShipperID);
            });

            builder.Entity<Include>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "include", PortalConst.DbSchema)
                .HasKey(i => new { i.TransactionID, i.ProductID });

                b.HasOne(i => i.Transaction)
                .WithMany(b => b.Includes)
                .HasForeignKey(i => i.TransactionID);

                b.HasOne(i => i.Product)
                .WithMany(p => p.Includes)
                .HasForeignKey(i => i.ProductID);
            });

            builder.Entity<Product>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "product", PortalConst.DbSchema);
            });
        }
    }

}
