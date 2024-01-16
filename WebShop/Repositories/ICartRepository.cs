using WebShop.Models;

namespace WebShop.Repositories
{
    public interface ICartRepository
    {
        Task<bool> AddItem(int productId, int qty);
        Task<bool> RemoveItem(int productId, int qty);
        Task<ShoppingCart> GetCart(string userId);
        Task<ShoppingCart> GetUserCart();

    }
}