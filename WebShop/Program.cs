using ProductService = WebShop.Repositories.Implement.ProductService;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WebShopDbContextConnection") ?? throw new InvalidOperationException("Connection string 'WebShopDbContextConnection' not found.");

builder.Services.AddDbContext<WebShopDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddDefaultUI()     
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<WebShopDbContext>();
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IHomeRepository, HomeRepository>();
builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<IUserOrderRepository, UserOrderRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
});     

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedDefaultData(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

//StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe: SecretKey").Get<string>();

app.UseAuthorization();
//app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();