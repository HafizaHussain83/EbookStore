

using BookShop.Models;
using BookShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BookShop.Repositories
{
    public class CartRepositories: ICartRepository
    {
        private readonly ApplicationDbContext _db;   // ✅ use ApplicationDbContext
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepositories(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<int> AddItem(int bookId, int qty)
        {
            string userId = GetUserId();
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not Logged in.");
                }

                var cart = await GetCart(userId);
                if (cart == null)
                {
                    cart = new ShoppingCart { UserId = userId };
                    _db.ShoppingCarts.Add(cart);
                    await _db.SaveChangesAsync();
                }

                var cartItem = await _db.CartDetails
                    .FirstOrDefaultAsync(ci => ci.ShoppingCartId == cart.Id && ci.BookId == bookId);

                if (cartItem != null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var books = _db.Books.Find(bookId);
                    cartItem = new CartDetails
                    {
                        ShoppingCartId = cart.Id,
                        BookId = bookId,
                        Quantity = qty,
                        UnitPrice= books.Price  //it is a new line after update 
                    };
                    _db.CartDetails.Add(cartItem);
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log the exception
                throw; // Re-throw the exception after rollback
            }

            return await GetCatItemCount(userId);
        }

        public async Task<int> RemoveItem(int bookId, int qty = 1)
        {
            string userId = GetUserId();
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not Logged in.");

                var cart = await GetCart(userId);   // ✅ await here
                if (cart == null)
                    throw new Exception("Invalid Cart");

                var cartItem = await _db.CartDetails
                    .FirstOrDefaultAsync(ci => ci.ShoppingCartId == cart.Id && ci.BookId == bookId);

                if (cartItem == null)
                    throw new Exception("No Item in cart");

                if (cartItem.Quantity > qty)
                {
                    cartItem.Quantity -= qty; // decrease quantity
                }
                else
                {
                    _db.CartDetails.Remove(cartItem); // remove completely
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return await GetCatItemCount(userId);  // ✅ await instead of .Result
        }



       
        public async Task<ShoppingCart> GetUserCart()
        {
            

            string userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid User");
            var cart = await _db.ShoppingCarts
                .Include(c => c.CartDetails) // Include cart details
                .ThenInclude(cd => cd.Book) // Include book details
               .ThenInclude(a=>a.Genre)
                .Where(c => c.UserId == userId).FirstOrDefaultAsync();
            return cart;


        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            return await _db.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<int> GetCatItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }

            return await (from cart in _db.ShoppingCarts
                          join details in _db.CartDetails
                          on cart.Id equals details.ShoppingCartId
                          where cart.UserId == userId
                          select details.Quantity)
                         .SumAsync();
        }


        public string GetUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                throw new Exception("User is not authenticated.");
            }

            var userId = _userManager.GetUserId(user);   
            if (userId == null)
            {
                throw new Exception("User ID not found.");
            }
            return userId;
        }

        public async Task<bool> CheckOut(CheckoutModel model )
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged");

                var cart = await GetCart(userId);
                if (cart == null)
                    throw new Exception("Invalid cart");

                var cartDetail = await _db.CartDetails
     .Include(cd => cd.Book)          // 👈 add this
     .Where(cd => cd.ShoppingCartId == cart.Id)
     .ToListAsync();                  // 👈 use async version


                if (cartDetail.Count == 0)
                    throw new Exception("No item in cart");
               var pendingresult = _db.OrderStatus.FirstOrDefault(o => o.StatusName == "Pending");
                if(pendingresult is null)
                {
                    throw new Exception("Order Status does not have pending status");

                }

                var order = new Orders
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    OrderStatusId = pendingresult.Id,
                    Name = model.Name,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    Address = model.Address,
                    IsPaid = false,
                    PaymentMethod = model.PaymentMethod
                };

                _db.Orders.Add(order);
                await _db.SaveChangesAsync();

                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        BookId = item.BookId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.Book.Price
                        // ✅ safe access
                    };
                    _db.OrderDetails.Add(orderDetail);
                }

                await _db.SaveChangesAsync();

                _db.CartDetails.RemoveRange(cartDetail);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


      

    }
}

