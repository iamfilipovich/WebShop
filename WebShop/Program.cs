    
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WebShopDbContextConnection") ?? throw new InvalidOperationException("Connection string 'WebShopDbContextConnection' not found.");

builder.Services.AddDbContext<WebShopDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()        
    .AddEntityFrameworkStores<WebShopDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IHomeRepository, HomeRepository>();
builder.Services.AddTransient<ICartRepository, CartRepository>();

builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
});     

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using(var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Member" };

    foreach (var role in roles)
    {
        if(!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string email = "admin@admin.com";
    string password = "admin123";
    string firstName = "Admin";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser
        {
            Email = email,
            FirstName = firstName,
            LastName = firstName,
            UserName = email,
            
        };

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");
    }
}

    app.Run();
