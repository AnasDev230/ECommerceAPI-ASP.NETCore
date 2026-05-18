using System.Reflection.Emit;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI_ASP.NETCore.Data
{
    public class EcommerceDBContext : IdentityDbContext<IdentityUser>
    {
        public EcommerceDBContext(DbContextOptions<EcommerceDBContext> dbContextOptions) : base(dbContextOptions)
        {

        }
public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Image)
                .WithMany()
                .HasForeignKey(p => p.ImageID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.CategoryId);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.VendorId);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.IsActive);

            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Stocks)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Image)
                .WithMany()
                .HasForeignKey(s => s.ImageID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Stock>()
                .HasIndex(s => s.ProductId);

            modelBuilder.Entity<Stock>()
                .HasIndex(s => s.SKU)
                .IsUnique()
                .HasFilter("[SKU] IS NOT NULL");

            modelBuilder.Entity<Category>()
                .HasOne(c => c.Image)
                .WithMany()
                .HasForeignKey(c => c.ImageID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.ParentCategoryId);

            modelBuilder.Entity<ShoppingCart>()
                .HasMany(c => c.Items)
                .WithOne(i => i.ShoppingCart)
                .HasForeignKey(i => i.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShoppingCart>()
                .HasIndex(c => c.CustomerId)
                .IsUnique();

            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(i => i.Stock)
                .WithMany()
                .HasForeignKey(i => i.StockId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShoppingCartItem>()
                .HasIndex(i => new { i.ShoppingCartId, i.StockId })
                .IsUnique();

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Ratings)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Customer)
                .WithMany()
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>()
                .HasIndex(r => new { r.CustomerId, r.ProductId })
                .IsUnique();

            modelBuilder.Entity<Rating>()
                .HasIndex(r => r.ProductId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.CustomerId);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.Status);

            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.Stock)
                .WithMany()
                .HasForeignKey(i => i.StockId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasIndex(i => i.OrderId);

modelBuilder.Entity<Image>()
                .HasIndex(i => i.Url);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Address>()
                .HasIndex(a => a.UserId);

            modelBuilder.Entity<Address>()
                .HasIndex(a => new { a.UserId, a.IsDefault })
                .HasFilter("[IsDefault] = 1");

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.OrderId)
                .IsUnique();

            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.Status);

            modelBuilder.Entity<Shipping>()
                .HasOne(s => s.Order)
                .WithOne(o => o.Shipping)
                .HasForeignKey<Shipping>(s => s.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Shipping>()
                .HasIndex(s => s.OrderId)
                .IsUnique();

            modelBuilder.Entity<Shipping>()
                .HasIndex(s => s.Status);

            modelBuilder.Entity<Shipping>()
                .HasIndex(s => s.TrackingNumber);

            modelBuilder.Entity<Shipping>()
                .HasOne(s => s.ShippingAddress)
                .WithMany()
                .HasForeignKey(s => s.ShippingAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuditLog>()
                .HasIndex(a => a.EntityType);

            modelBuilder.Entity<AuditLog>()
                .HasIndex(a => a.EntityId);

            modelBuilder.Entity<AuditLog>()
                .HasIndex(a => a.Action);

            modelBuilder.Entity<AuditLog>()
                .HasIndex(a => a.CreatedAt);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.BillingAddress)
                .WithMany()
                .HasForeignKey(o => o.BillingAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Payment)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.PaymentId);

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.GatewayTransactionId)
                .IsUnique()
                .HasFilter("[GatewayTransactionId] IS NOT NULL");

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.Status);
        }
    }
}
