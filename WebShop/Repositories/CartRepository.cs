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
        public async Task<bool> AddItem(int productId, int qty)
        {
            using var transaction = _db.Database.BeginTransaction();
            try { 
                string userId = GetUserId();
                if(userId == null)
                    return false;

                var cart = await GetCart(userId);
                if(cart == null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId,
                    };
                    _db.ShoppingCarts.Add(cart);
                }
                _db.SaveChanges();
                
                var cartItem = _db.CartDetails.FirstOrDefault(c => c.ProductId == productId && c.ShoppingCartId == cart.Id);
                if( cartItem != null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
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
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> RemoveItem(int productId, int qty)
        {
            try
            {
                string userId = GetUserId();
                if (userId == null)
                    return false;

                var cart = await GetCart(userId);
                if (cart == null)
                {
                    return false;
                }
                //remove items
                var cartItem = _db.CartDetails.FirstOrDefault(c => c.ProductId == productId && c.ShoppingCartId == cart.Id);
                if (cartItem == null)
                    return false;
                else if(cartItem.Quantity == 1)
                {
                    _db.CartDetails.Remove(cartItem);
                }else
                {
                    cartItem.Quantity--;
                }
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }
        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid user id");
            var shoppingCart = await _db.ShoppingCarts.Include(a => a.CartDetails)
                                                .ThenInclude(a => a.Product)
                                                .ThenInclude(a => a.Category)
                                                .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;
        }
        public string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
