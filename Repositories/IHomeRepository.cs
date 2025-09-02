using BookShop.Models;
namespace BookShop
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Book>> DisplayBooks(string sTerm = "", int categoryId = 0);
        Task<IEnumerable<Genre>> Genres();
    }
}