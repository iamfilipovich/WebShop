using Microsoft.AspNetCore.Mvc;
using Stripe.BillingPortal;
using Stripe.Checkout;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace WebShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;
        private readonly WebShopDbContext _db;
        private readonly IProductService _productService;

        public CartController(ICartRepository cartRepo, WebShopDbContext db, IProductService productService)
        {
            _cartRepo = cartRepo;
            _db = db;
            _productService = productService;
        }

        private ILog log = LogManager.GetLogger("CartController");
        public async Task<IActionResult> Charge()
        {
            // Replace with your actual Stripe API key
            StripeConfiguration.ApiKey = "sk_test_51OttqFEXxriyRJQtbOacC2yliaODneUBvM2iRo2fNFK0Ubl3fmj1PUl1SwWbDUrYyJSOnYb4uWUU3vt4tDJm1FFe00gN0KIsOE";

            // Retrieve product information from the ProductService
            var shoppingCart = await _cartRepo.GetUserCart();
            var cartItems = shoppingCart.CartDetails;

            // Convert cart items to line items
            var lineItems = new List<SessionLineItemOptions>();
            foreach (var cartItem in cartItems)
            {
                var product = cartItem.Product;
                if (product != null)
                {
                    var lineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "eur",
                            UnitAmount = (long)(product.Price * 100), // Amount in cents
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = product.Name,
                            },
                        },
                        Quantity = cartItem.Quantity,
                    };
                    lineItems.Add(lineItem);
                }
            }

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment", // Specify the mode for one-time payment
                SuccessUrl = "http://localhost:17671/order/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "http://localhost:17671/order/cancel?session_id={CHECKOUT_SESSION_ID}",

                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Metadata = new Dictionary<string, string>
                    {
                        {"Order_ID", "123456" },
                        {"Description", "Comfortable" }
                    }
                }
            };

            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            // Redirect the user to the Stripe checkout page
            return Redirect(session.Url);
        }

        [HttpGet("/order/success")]
        public async Task<IActionResult> Thanks()
        {
            // Reduce the quantity of each product in the cart
            var cart = await _cartRepo.GetUserCart();
            foreach (var cartItem in cart.CartDetails)
            {
                await _productService.ReduceQuantity(cartItem.ProductId, cartItem.Quantity);
            }

            // Perform checkout
            bool successful = await _cartRepo.DoCheckout();

            // Redirect to the appropriate view
            if (successful)
            {
                return View();
            }
            else
            {
                // Handle unsuccessful checkout
                return View("CheckoutFailed");
            }
        }

        [HttpGet("/order/cancel")]
        public IActionResult Cancel()
        {
            return View();
        }
        public async Task<IActionResult> AddItem(int productId, int qty = 1, int redirect = 0)
        {
            var product = _db.Products.Find(productId);
            if (product == null || product.StockQuantity < qty)
            {
                return Json(new { success = false, insufficientStock = true, message = "Insufficient stock." });
            }
            var cartCount = await _cartRepo.AddItem(productId, qty);
            if (redirect == 0)
            {
                // Return the updated cart count in the JSON response
                return Json(new { success = true, cartCount = cartCount });
            }
            return RedirectToAction("GetUserCart");
        }



        public async Task<IActionResult> RemoveItem(int productId)
        {
            var cartCount = await _cartRepo.RemoveItem(productId);

            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepo.GetUserCart();

            return View(cart);
        }

        public int GetStockQuantity(int productId)
        {
            // Retrieve the product from the database
            var product = _db.Products.FirstOrDefault(p => p.ProductID == productId);

            // Return the stock quantity if the product is found, otherwise return 0
            return product != null ? product.StockQuantity : 0;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItemCount()
        {
            try
            {
                var itemCount = await _cartRepo.GetCartItemCount();
                return Ok(itemCount);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
