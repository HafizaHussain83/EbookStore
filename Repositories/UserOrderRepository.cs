using BookShop.Models;
using Microsoft.AspNetCore.Identity;

namespace BookShop.Repositories
{
    public class UserOrderRepository:IUserOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public UserOrderRepository(ApplicationDbContext db , IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<IEnumerable<Orders>> UserOrders()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User Is not Logged-in");
            var orders = await _db.Orders
                    .Include(o => o.OrderStatus)
                    .Include(o => o.OrderDetail)
                    .ThenInclude(od => od.Book)
                     .ThenInclude(b => b.Genre)
    
    .Where(o => o.UserId == userId)
    .ToListAsync();



            //var orders = await _db.Orders
            //    .Include(o => o.OrderDetail) 
            //    .Include(o => o.OrderStatus)
            //   .ThenInclude(o=>o.Book)
            //   .ThenInclude(b => b.Genre)
            //    .Where(o => o.UserId == userId)
            //    .ToListAsync();

            return orders;
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
