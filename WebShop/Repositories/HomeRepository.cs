using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly WebShopDbContext _db;

        public HomeRepository(WebShopDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> Categories()
        {
            return await _db.Category.ToListAsync();
        }

        public async Task<IEnumerable<Products>> GetProducts(string sTerm = "", int CategoryID = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Products> products = await (from product in _db.Products join
                            category in _db.Category on
                            product.CategoryID equals category.CategoryID
                            where string.IsNullOrWhiteSpace(sTerm) || (product != null && product.Name.ToLower().StartsWith(sTerm))
                            select new Products
                            {
                                ProductID = product.ProductID,
                                Name = product.Name,
                                Price = product.Price,
                                Seller = product.Seller,
                                CategoryID = category.CategoryID,
                                CategoryName = category.Name
                            }
                            ).ToListAsync();
            if(CategoryID > 0)
            {
                products = products.Where(a => a.CategoryID == CategoryID).ToList();
            }
            return products;
        }
    }
}
