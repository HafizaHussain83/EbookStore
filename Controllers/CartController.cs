using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookShop.Models.DTOs;
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
        public  IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {
            if (!ModelState.IsValid)
            {
                
                return View(model);
            }

            bool isCheckout = await _cartRepo.CheckOut(model);
            if (!isCheckout)
                return RedirectToAction(nameof(OrderFailur));

            return RedirectToAction(nameof(OrderSucces));
        }

        public IActionResult OrderSucces()
        {
            return View();
        }
        public IActionResult OrderFailur()
        {
            return View();
        }
    }
}
