using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;

        public CartController(ICartRepository cartRepo )
        {
          _cartRepo = cartRepo;
        }

        public  async Task<IActionResult> AddItem( int bookId,int qty=1,int redrict=0 )
        {
            var cartCount = await _cartRepo.AddItem(bookId, qty);
            if (redrict == 0)

                return Ok(cartCount);
            return RedirectToAction("GetUserCart");     
        }

        public async Task<IActionResult> RemoveItem(int bookId )
        {
            var cartCount = await _cartRepo.RemoveItem(bookId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart(int bookId, int qty = 1)
        {
            var cart = await _cartRepo.GetUserCart();

            return View(cart);
        }
        public async Task<IActionResult> GetTotalItemCart(int bookId, int qty = 1)
        {
            var cartitem= await _cartRepo.GetCatItemCount();
            return Ok(cartitem);
        }

    }
}
