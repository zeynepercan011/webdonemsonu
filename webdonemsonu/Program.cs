using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using webdonemsonu.Data; // ApplicationDbContext burada
using webdonemsonu.Data.Repositories; // IProductRepository ve ProductRepository burada
using webdonemsonu.Models; // ApplicationUser burada
using webdonemsonu.Services; // ICartService, CartService, IImageService, ImageService burada
using webdonemsonu.Components; // CartItemCountViewComponent burada

internal class Program
{
	private static async Task Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// DbContext (PostgreSQL)
		builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

		// Identity ayarlarý
		builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
		{
			options.SignIn.RequireConfirmedAccount = false;
			options.Password.RequireDigit = true;
			options.Password.RequiredLength = 6;
			options.Password.RequireNonAlphanumeric = false;
			options.Password.RequireUppercase = false;
		})
		.AddRoles<IdentityRole>()
		.AddEntityFrameworkStores<ApplicationDbContext>();

		// Repository'leri ekleyin
		builder.Services.AddScoped<IProductRepository, ProductRepository>();  // Product Repository ekleniyor

		// Servisleri ekleyin
		builder.Services.AddScoped<ICartService, CartService>();  // Sepet Servisi
		builder.Services.AddScoped<IImageService, ImageService>();  // Resim Servisi
		builder.Services.AddScoped<IOrderRepository, OrderRepository>();

		// ViewComponent'ý kaydedin
		builder.Services.AddTransient<CartItemCountViewComponent>();

		// HttpContext eriþimi için
		builder.Services.AddHttpContextAccessor();

		// Session'ý etkinleþtirin (sepet vs için kullanýlacak)
		builder.Services.AddSession(options =>
		{
			options.IdleTimeout = TimeSpan.FromMinutes(30);
			options.Cookie.HttpOnly = true;
			options.Cookie.IsEssential = true;
		});

		// MVC Controller + View servisi
		builder.Services.AddControllersWithViews();

		var app = builder.Build();

		// DbInitializer çalýþtýr
		using (var scope = app.Services.CreateScope())
		{
			var services = scope.ServiceProvider;
			try
			{
				var context = services.GetRequiredService<ApplicationDbContext>();
				var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
				var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
				var env = services.GetRequiredService<IWebHostEnvironment>();

				await DbInitializer.InitializeAsync(context, userManager, roleManager, env);
			}
			catch (Exception ex)
			{
				var logger = services.GetRequiredService<ILogger<Program>>();
				logger.LogError(ex, "Veritabaný baþlatýlýrken bir hata oluþtu!");
			}
		}

		// Middleware pipeline
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles(); // wwwroot dosyalarý için gerekli

		app.UseRouting();

		app.UseAuthentication(); // Login için gerekli
		app.UseAuthorization();  // Yetkilendirme için gerekli

		app.UseSession(); //  Artýk Session kullanýlabilecek

		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}");

		app.Run();
	}
}
