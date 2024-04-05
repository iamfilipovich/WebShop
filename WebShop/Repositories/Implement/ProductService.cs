using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;
using WebShop.Repositories.Abstract;

namespace WebShop.Repositories.Implement
{
    
    public class ProductService : IProductService
    {
        private readonly WebShopDbContext _db;

        public ProductService(WebShopDbContext db)
        {
            _db = db;
        }
        public bool Add(Products model)
        {
            try
            {
                _db.Products.Add(model);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = FindById(id);
                if (data == null)
                {
                    return false;
                }
                _db.Products.Remove(data);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Products FindById(int id)
        {
            var product = _db.Products
                .Include(p => p.Category) // Include related category
                .FirstOrDefault(p => p.ProductID == id);

            return product;
        }


        public IEnumerable<Products> GetAll()
        {
            var data = (from product in _db.Products
                        join category in _db.Category
                        on product.CategoryID equals category.CategoryID
                        select new Products
                        {
                            ProductID = product.ProductID,
                            CategoryID = product.CategoryID,
                            CategoryName = category.Name,
                            Description = product.Description,
                            StockQuantity = product.StockQuantity,
                            Price = product.Price,
                            Image = product.Image,
                            Name = product.Name
                        }).ToList();
            return data;
        }

        public bool Update(Products model)
        {
            try
            {
                _db.Products.Update(model);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ReduceQuantity(int productId, int qty)
        {
            try
            {
                var product = _db.Products.Find(productId);
                if (product == null)
                {
                    // Product not found
                    return false;
                }
                if (product.StockQuantity < qty)
                {
                    // Insufficient stock
                    return false;
                }
                // Reduce the quantity
                product.StockQuantity -= qty;
                // Save changes to the database
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                // Log the error or handle it as needed
                return false;
            }
        }
    }
}
