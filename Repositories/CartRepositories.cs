

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
                    cartItem = new CartDetails
                    {
                        ShoppingCartId = cart.Id,
                        BookId = bookId,
                        Quantity = qty
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


        //public async Task<int>AddItem(int bookId, int qty)
        //{
        //    string userId = GetUserId();

        //    using var transaction = await _db.Database.BeginTransactionAsync();
        //    try
        //    {

        //        if (string.IsNullOrEmpty(userId))
        //        {
        //           throw new Exception("User is not Logged in.");
        //        }

        //        var cart = await  GetCart(userId);

        //        if (cart == null)
        //        {
        //            cart = new ShoppingCart
        //            {
        //                UserId = userId,
        //            };
        //            _db.ShoppingCarts.Add(cart);
        //            await _db.SaveChangesAsync();
        //        }

        //        var cartItem = _db.CartDetails.FirstOrDefault(ci => ci.ShoppingCartId == cart.Id && ci.BookId == bookId);

        //        if (cartItem != null)
        //        {
        //            cartItem.Quantity += qty;   // ✅ fixed property name
        //        }
        //        else
        //        {
        //            cartItem = new CartDetails
        //            {
        //                ShoppingCartId = cart.Id,
        //                BookId = bookId,
        //                Quantity = qty
        //            };
        //            _db.CartDetails.Add(cartItem);
        //        }

        //        await _db.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //    }
        //    catch (Exception)
        //    {
        //      //  await transaction.RollbackAsync();

        //    }
        //    var cartItemCount = GetCatItemCount(userId);
        //    return cartItemCount.Result;
        //}
        public async Task<int> RemoveItem(int bookId, int qty = 1)
        {
            string userId = GetUserId();
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {               
                if (string.IsNullOrEmpty(userId))
                {    throw new Exception("User is not Logged in.");
                }
                var cart = GetCart(userId);
                if (cart == null)
                { throw new Exception(" Invalid Cart"); // no cart to remove from
                }
                var cartItem = _db.CartDetails.FirstOrDefault(ci => ci.ShoppingCartId == cart.Id && ci.BookId == bookId);
                if (cartItem == null)
                {                    throw new Exception("No Item in cart");// item not in cart
                }
                if (cartItem.Quantity > qty)
                {    cartItem.Quantity -= qty; // decrease quantity
                }  else
                { _db.CartDetails.Remove(cartItem); // remove completely
                }
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {    
                //   await transaction.RollbackAsync();
            }
            var cartItemCount = GetCatItemCount(userId);
            return cartItemCount.Result;
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
    }
}

