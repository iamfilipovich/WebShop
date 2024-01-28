using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebShop.Areas.Identity.Data;
using WebShop.Models;
using WebShop.Models.DTOs;

namespace WebShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository, UserManager<ApplicationUser> userManager)
        {
            _homeRepository = homeRepository;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string sterm = "", int CategoryID = 0)
        {
            IEnumerable<Products> products = await _homeRepository.GetProducts(sterm, CategoryID);
            IEnumerable<Category> categories = await _homeRepository.Categories();
            ProductDisplayModel productModel = new()
            {
                Products = products,
                Categories = categories,
                STerm = sterm,
                categoryID = CategoryID
            };

            return View(productModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Member")]
        public IActionResult Create()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}