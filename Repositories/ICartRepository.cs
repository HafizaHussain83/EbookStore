using BookShop.Models;

namespace BookShop.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int bookId, int qty);
        Task<int> RemoveItem(int bookId, int qty = 1);
        Task<ShoppingCart> GetUserCart();
        Task<int> GetCatItemCount(string userId = "");
        Task<ShoppingCart> GetCart(string userId);
        Task<bool> CheckOut(CheckoutModel model);

    }
}
