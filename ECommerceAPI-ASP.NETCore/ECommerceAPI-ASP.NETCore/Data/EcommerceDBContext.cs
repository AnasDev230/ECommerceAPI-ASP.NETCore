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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var adminRoleId = "e7233f4e-8897-499a-9f5f-fbaf10e35dcf";
            var customerRoleId = "c7c74801-7e0b-419a-a441-bfe8a699425d";
            var vendorRoleId = "430a9028-809a-460e-9e4d-a86b7e628407";

            var roles = new List<IdentityRole>
    {
        new IdentityRole
        {
            Id = adminRoleId,
            ConcurrencyStamp = adminRoleId,
            Name = "Admin",
            NormalizedName = "ADMIN"
        },
        new IdentityRole
        {
            Id = customerRoleId,
            ConcurrencyStamp = customerRoleId,
            Name = "Customer",
            NormalizedName = "CUSTOMER"
        },
        new IdentityRole
        {
            Id = vendorRoleId,
            ConcurrencyStamp = vendorRoleId,
            Name = "Vendor",
            NormalizedName = "VENDOR"
        }
    };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId);


            var electronicsId = Guid.NewGuid();
            var fashionId = Guid.NewGuid();
            var mobilesId = Guid.NewGuid();
            var laptopsId = Guid.NewGuid();
            var menClothingId = Guid.NewGuid();
            var womenClothingId = Guid.NewGuid();

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = electronicsId, Name = "Electronics", UrlHandle = "electronics", ParentCategoryId = null },
                new Category { Id = mobilesId, Name = "Mobiles", UrlHandle = "mobiles", ParentCategoryId = electronicsId },
                new Category { Id = laptopsId, Name = "Laptops", UrlHandle = "laptops", ParentCategoryId = electronicsId },
                new Category { Id = fashionId, Name = "Fashion", UrlHandle = "fashion", ParentCategoryId = null },
                new Category { Id = menClothingId, Name = "Men's Clothing", UrlHandle = "mens-clothing", ParentCategoryId = fashionId },
                new Category { Id = womenClothingId, Name = "Women's Clothing", UrlHandle = "womens-clothing", ParentCategoryId = fashionId }
            );



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

        }
    }
}
