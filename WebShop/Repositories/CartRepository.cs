using Microsoft.EntityFrameworkCore.Infrastructure;
using WebShop.Models;

namespace WebShop.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly WebShopDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(WebShopDbContext db, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> AddItem(int productId, int qty)
        {
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction();
            try { 
                if(string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged.");

                var cart = await GetCart(userId);
                if(cart == null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId,
                    };
                    _db.ShoppingCart.Add(cart);
                }
                _db.SaveChanges();
                
                var cartItem = _db.CartDetails.FirstOrDefault(c => c.ProductId == productId && c.ShoppingCartId == cart.Id);
                if( cartItem != null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var product = _db.Products.Find(productId);
                    cartItem = new CartDetail 
                    { 
                        ProductId = productId,
                        ShoppingCartId = cart.Id,
                        Quantity = qty
                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                transaction.Commit();   
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;   
        }
        public async Task<int> RemoveItem(int productId)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged.");

                var cart = await GetCart(userId);
                if (cart == null)
                {
                    throw new Exception("Cart is empty.");
                }
                //remove items
                var cartItem = _db.CartDetails.FirstOrDefault(c => c.ProductId == productId && c.ShoppingCartId == cart.Id);
                if (cartItem == null)
                    throw new Exception("No items in cart.");
                else if(cartItem.Quantity == 1)
                {
                    _db.CartDetails.Remove(cartItem);
                }else
                {
                    cartItem.Quantity--;
                }
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }
        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid user id");
            var shoppingCart = await _db.ShoppingCart.Include(a => a.CartDetails)
                                                .ThenInclude(a => a.Product)
                                                .ThenInclude(a => a.Category)
                                                .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;
        }
        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }

            var data = await (from cart in _db.ShoppingCart 
                              join cartDetail in _db.CartDetails on 
                              cart.Id equals cartDetail.ShoppingCartId
                              select new { cartDetail.Id }).ToListAsync();
            return data.Count;
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
