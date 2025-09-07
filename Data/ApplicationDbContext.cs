

using BookShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // OrderDetail → Orders (one-to-many)
            builder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetail)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderDetail → Books (one-to-many)
            builder.Entity<OrderDetail>()
                .HasOne(od => od.Book)
                .WithMany(b => b.OrderDetail)
                .HasForeignKey(od => od.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // CartDetails → ShoppingCart (one-to-many)
            // FIXED: Added navigation property reference
            builder.Entity<CartDetails>()
                .HasOne(cd => cd.ShoppingCart)
                .WithMany(sc => sc.CartDetails) // ← Add this navigation property
                .HasForeignKey(cd => cd.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

            // CartDetails → Books (one-to-many)
            builder.Entity<CartDetails>()
                .HasOne(cd => cd.Book)
                .WithMany(b => b.CartDetail)
                .HasForeignKey(cd => cd.BookId)
                .OnDelete(DeleteBehavior.Restrict);
        }



        






    }
}
