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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           


            modelBuilder.Entity<Product>()
    .HasOne(p => p.Category)
    .WithMany(c => c.Products)
    .HasForeignKey(p => p.CategoryId)
    .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Stock>()
    .HasOne(s => s.Product)
    .WithMany(p => p.Stocks)
    .HasForeignKey(s => s.ProductId)
    .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<ShoppingCart>()
    .HasMany(c => c.Items)
    .WithOne(i => i.ShoppingCart)
    .HasForeignKey(i => i.ShoppingCartId)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShoppingCartItem>()
    .HasOne(i => i.Stock)
    .WithMany()
    .HasForeignKey(i => i.StockId)
    .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Rating>()
       .HasOne(r => r.Product)
       .WithMany(p => p.Ratings)
       .HasForeignKey(r => r.ProductId);

            modelBuilder.Entity<Rating>()
       .HasOne(r => r.Customer)
       .WithMany()
       .HasForeignKey(r => r.CustomerId);

            modelBuilder.Entity<Rating>()
        .HasIndex(r => new { r.CustomerId, r.ProductId })
        .IsUnique();



            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId);

            modelBuilder.Entity<OrderItem>()
              .HasOne(i => i.Stock)
              .WithMany()
              .HasForeignKey(i => i.StockId);

            
        }
    }
}
