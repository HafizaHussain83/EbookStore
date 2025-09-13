using BookShop.Models;

namespace BookShop.Repositories
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Orders>> UserOrders();

    }
}