using Microsoft.AspNetCore.Mvc;

namespace WebShop.Controllers
{
    [Authorize]
    public class UserOrderController : Controller
    {
        private readonly IUserOrderRepository _userOrderRepo;

        public UserOrderController(IUserOrderRepository userOrderRepo)
        {
            _userOrderRepo = userOrderRepo;
        }

        public async Task<IActionResult> GetAllUserOrders()
        {
            var allOrders = await _userOrderRepo.GetAllOrdersWithUser();
            return View(allOrders);
        }
        public async Task<IActionResult> UserOrders()
        {
            var orders = await _userOrderRepo.UserOrders();
            return View(orders);
        }

        
    }
}
