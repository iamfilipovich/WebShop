﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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
    }

    public DbSet<Products> Products { get; set; } = default!;
    public DbSet<Category> Category { get; set; } = default!;
    public DbSet<CartDetail> CartDetails { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderDetail> OrderDetails { get; set; } = default!;
    public DbSet<OrderStatus> OrderStatuses { get; set; } = default!;
    public DbSet<ShoppingCart> ShoppingCart { get; set; } = default!;

}
