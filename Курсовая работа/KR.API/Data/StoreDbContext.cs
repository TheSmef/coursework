using Kr.Models;
using KR.Models;
using Microsoft.EntityFrameworkCore;

namespace KR.API.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StorageHistory>().HasOne(e => e.ProductStorage).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Purchase>().HasOne(e => e.ProductStorage).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Purchase>().HasOne(e => e.PurchaseAgreement).WithMany(x => x.Purchases).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Order>().HasOne(e => e.UserPost).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderProduct>().HasOne(e => e.Product).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderProduct>().HasOne(e => e.Order).WithMany(x => x.OrderProducts).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Role>().HasOne(e => e.AccountUser).WithMany(x => x.Roles).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductStorage>().HasOne(e => e.Category).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserPost>().HasOne(e => e.User).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserPost>().HasOne(e => e.Post).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SalaryHistory>().HasOne(e => e.UserPost).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Account>().HasOne(e => e.User).WithOne(x => x.Account).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StorageHistory>().HasOne(e => e.ProductStorage).WithMany().IsRequired(true);

            modelBuilder.Entity<User>().Property(e => e.Otch).IsRequired(false);


            modelBuilder.Entity<Post>().Property(p => p.Salary).HasPrecision(15, 2);
            modelBuilder.Entity<ProductStorage>().Property(p => p.Cost).HasPrecision(15, 2);
            modelBuilder.Entity<OrderProduct>().Property(p => p.Price).HasPrecision(15, 2);
            modelBuilder.Entity<UserPost>().Property(p => p.Share).HasPrecision(3, 2);
            modelBuilder.Entity<Purchase>().Property(p => p.Price).HasPrecision(15, 2);
            modelBuilder.Entity<SalaryHistory>().Property(p => p.Payment).HasPrecision(15, 2);

            modelBuilder.Entity<User>().Property(e => e.Otch).IsRequired(false);
            modelBuilder.Entity<Account>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Account>().HasIndex(u => u.Login).IsUnique();
            modelBuilder.Entity<Account>(e => e.HasCheckConstraint("CH_Email", "Email like '%@%.%'"));

            modelBuilder.Entity<Post>(e => e.HasCheckConstraint("CH_Salary_Post", "Salary > 0"));
            modelBuilder.Entity<Post>().HasIndex(u => u.Name).IsUnique();

            modelBuilder.Entity<ProductStorage>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<ProductStorage>(e => e.HasCheckConstraint("CH_Cost_Product", "Cost > 0"));
            modelBuilder.Entity<ProductStorage>(e => e.HasCheckConstraint("CH_Exrire_Product", "Exipiration_Date >= 0"));
            modelBuilder.Entity<ProductStorage>(e => e.HasCheckConstraint("CH_Amount_Product", "Amount >= 0"));

            modelBuilder.Entity<Purchase>(e => e.HasCheckConstraint("CH_Price_Puchase", "Price > 0"));
            modelBuilder.Entity<Purchase>(e => e.HasCheckConstraint("CH_Amount_Purchase", "Amount > 0"));

            modelBuilder.Entity<UserPost>(e => e.HasCheckConstraint("Ch_Share_UserPost", "Share > 0 AND Share <= 1"));

            modelBuilder.Entity<SalaryHistory>(e => e.HasCheckConstraint("CH_Payment_Salary", "Payment > 0"));

            modelBuilder.Entity<OrderProduct>(e => e.HasCheckConstraint("CH_Amount_Order", "Amount > 0"));
            modelBuilder.Entity<OrderProduct>(e => e.HasCheckConstraint("CH_Price_Order", "Price > 0"));

            modelBuilder.Entity<Category>().HasIndex(u => u.Name).IsUnique();


        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPost> UserPosts { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<ProductStorage> ProductStorages { get; set; }
        public DbSet<SalaryHistory> SalaryHistories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PurchaseAgreement> PurchaseAgreements { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<StorageHistory> StorageHistory { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
