using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebShop.Areas.Identity.Data;
using WebShop.Models;

namespace WebShop.Data;

public class WebShopDbContext : IdentityDbContext<ApplicationUser> 
{
    public WebShopDbContext(DbContextOptions<WebShopDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Products> Products { get; set; } = default!;
    public DbSet<Category> Category { get; set; } = default!;
    public DbSet<CartDetail> CartDetails{ get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = default!;
    public DbSet<OrderStatus> OrderStatuses { get; set; } = default!;
    public DbSet<ShoppingCart> ShoppingCarts { get; set; } = default!;

}
