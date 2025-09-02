//using BookShop.Models;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;

//namespace BookShop.Data
//{
//    public class ApplicationDbContext : IdentityDbContext
//    {
//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//            : base(options)
//        {
//        }
//        public DbSet<Book> Books { get; set; }
//        public DbSet<Genre> Genres { get; set; }
//        public DbSet<Orders> Orders { get; set; }
//        public DbSet<OrderDetail> OrderDetails { get; set; }
//        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
//        public DbSet<CartDetails> CartDetails { get; set; }
//        public DbSet<OrderStatus> OrderStatus { get; set; }


//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);

//            // Prevent multiple cascade paths
//            builder.Entity<Book>()
//                .HasOne(o => o.Order)
//                .WithMany()
//                .HasForeignKey(o => o.OrderId)
//                .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.NoAction

//            builder.Entity<OrderDetail>()
//                .HasOne(od => od.Order)
//                .WithMany()
//                .HasForeignKey(od => od.OrderId)
//                .OnDelete(DeleteBehavior.Cascade); // keep cascade here

//            builder.Entity<OrderDetail>()
//                .HasOne(od => od.Book)
//                .WithMany()
//                .HasForeignKey(od => od.BookId)
//                .OnDelete(DeleteBehavior.Restrict); // prevent cascade loop
//        }


//    }
//}


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

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);

        //    // Book → Orders (one-to-many)
        //    builder.Entity<Book>()
        //        .HasOne(b => b.Order)
        //        .WithMany(o => o.Books) // 👈 use Orders.Books
        //        .HasForeignKey(b => b.OrderId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    // OrderDetail → Orders (one-to-many)
        //    builder.Entity<OrderDetail>()
        //        .HasOne(od => od.Order)
        //        .WithMany(o => o.OrderDetail) // 👈 use Orders.OrderDetail
        //        .HasForeignKey(od => od.OrderId)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    // OrderDetail → Books (one-to-many)
        //    builder.Entity<OrderDetail>()
        //        .HasOne(od => od.Book)
        //        .WithMany(b => b.OrderDetail) // 👈 use Book.OrderDetail
        //        .HasForeignKey(od => od.BookId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //    // CartDetails → ShoppingCart (one-to-many)
        //    builder.Entity<CartDetails>()
        //        .HasOne(cd => cd.ShoppingCart)
        //        .WithMany() // you don't have ShoppingCart.CartDetails
        //        .HasForeignKey(cd => cd.ShoppingCartId)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    // CartDetails → Books (one-to-many)
        //    builder.Entity<CartDetails>()
        //        .HasOne(cd => cd.Book)
        //        .WithMany(b => b.CartDetail) // 👈 use Book.CartDetail
        //        .HasForeignKey(cd => cd.BookId)
        //        .OnDelete(DeleteBehavior.Restrict);
        //}



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Book → Orders (REMOVE this, Books should not directly link to Orders)
            // You don't need Book.OrderId & Book.Order navigation.
            // Orders are linked to Books through OrderDetail.

            // OrderDetail → Orders (one-to-many)
            builder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetail)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);  // keep cascade here

            // OrderDetail → Books (one-to-many)
            builder.Entity<OrderDetail>()
                .HasOne(od => od.Book)
                .WithMany(b => b.OrderDetail)
                .HasForeignKey(od => od.BookId)
                .OnDelete(DeleteBehavior.Restrict); // prevent cascade here

            // CartDetails → ShoppingCart (one-to-many)
            builder.Entity<CartDetails>()
                .HasOne(cd => cd.ShoppingCart)
                .WithMany()
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
