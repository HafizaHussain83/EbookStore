using BookShop.Models;

using System.Runtime.CompilerServices;

namespace BookShop.Repositories
{
    public class HomeRepository: IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Genre>>  Genres()
        {
            return await _db.Genres.ToListAsync();
        }
        public async Task<IEnumerable<Book>> DisplayBooks(string sTerm = "", int categoryId = 0)
        {
            sTerm = sTerm.ToLower();

            IEnumerable<Book> books = await (
                from book in _db.Books
                join genre in _db.Genres
                on book.GenreId equals genre.Id
                where string.IsNullOrWhiteSpace(sTerm)
                      || book.BookName.ToLower().StartsWith(sTerm)
                      || book.AuthorName.ToLower().Contains(sTerm)
                select new Book
                {
                    Id = book.Id,
                    BookName = book.BookName,
                    AuthorName = book.AuthorName,
                    GenreId = book.GenreId,
                    GenreName = genre.GenreName,
                    Image = book.Image,
                    Price = book.Price
                }
            ).ToListAsync();

            if (categoryId > 0)
            {

                books = books.Where(b => b.GenreId == categoryId).ToList();

            }

            return books;
        }
    }
}
