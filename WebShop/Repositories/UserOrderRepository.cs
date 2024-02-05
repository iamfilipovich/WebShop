using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebShop.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly WebShopDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserOrderRepository(WebShopDbContext db, IHttpContextAccessor httpContextAccessor, 
                                   UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Order>> UserOrders()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged");
            var orders = await _db.Orders.Include(a => a.OrderStatus)
                                   .Include(a => a.OrderDetail)
                                   .ThenInclude(a => a.Product)
                                   .ThenInclude(a => a.Category)
                                   .Where(a => a.UserId == userId)
                                   .ToListAsync();
            return orders;
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
