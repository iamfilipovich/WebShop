using Microsoft.AspNetCore.Mvc;
using WebShop.Models;

namespace WebShop.Repositories
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Products>> GetProducts(string sTerm ="", int CategoryID = 0);
        Task<IEnumerable<Category>> Categories();
    }
}