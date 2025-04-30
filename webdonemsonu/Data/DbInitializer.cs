using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using webdonemsonu.Models;

namespace webdonemsonu.Data
{
	public static class DbInitializer
	{
		public static async Task InitializeAsync(ApplicationDbContext context,
											  UserManager<ApplicationUser> userManager,
											  RoleManager<IdentityRole> roleManager,
											  IWebHostEnvironment env)
		{
			// 1. Veritabanı kontrolü
			await context.Database.EnsureCreatedAsync();

			// 2. Roller
			await CreateRolesAsync(roleManager);

			// 3. Kullanıcılar
			await CreateUsersAsync(userManager);

			// 4. Kategoriler
			if (!context.Categories.Any())
			{
				await CreateCategoriesAsync(context);
			}

			// 5. Ürünler (GERÇEK RESİMLERLE)
			if (!context.Products.Any())
			{
				await CreateProductsWithRealImagesAsync(context, env);
			}
		}

		private static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
		{
			if (!await roleManager.RoleExistsAsync("Admin"))
				await roleManager.CreateAsync(new IdentityRole("Admin"));

			if (!await roleManager.RoleExistsAsync("User"))
				await roleManager.CreateAsync(new IdentityRole("User"));
		}

		private static async Task CreateUsersAsync(UserManager<ApplicationUser> userManager)
		{
			if (await userManager.FindByEmailAsync("admin@shopier.com") == null)
			{
				var admin = new ApplicationUser
				{
					UserName = "admin@shopier.com",
					Email = "admin@shopier.com",
					FirstName = "Admin",
					LastName = "User",
					EmailConfirmed = true
				};
				await userManager.CreateAsync(admin, "Admin123!");
				await userManager.AddToRoleAsync(admin, "Admin");
			}

			if (await userManager.FindByEmailAsync("user@shopier.com") == null)
			{
				var user = new ApplicationUser
				{
					UserName = "user@shopier.com",
					Email = "user@shopier.com",
					FirstName = "Test",
					LastName = "User",
					EmailConfirmed = true
				};
				await userManager.CreateAsync(user, "User123!");
				await userManager.AddToRoleAsync(user, "User");
			}
		}

		private static async Task CreateCategoriesAsync(ApplicationDbContext context)
		{
			var categories = new List<Category>
			{
				new() { Name = "Erkek Giyim" },
				new() { Name = "Kadın Giyim" },
				new() { Name = "Çocuk Giyim" }
			};
			await context.Categories.AddRangeAsync(categories);
			await context.SaveChangesAsync();
		}

		private static async Task CreateProductsWithRealImagesAsync(ApplicationDbContext context, IWebHostEnvironment env)
		{
			var categories = await context.Categories.ToListAsync();
			var imagePath = Path.Combine(env.WebRootPath, "images/products");

			var predefinedProducts = new List<Product>
	{
		new()
		{
			Name = "Erkek Slim Fit Gömlek",
			Description = "%100 Pamuk, Beyaz Renk",
			Price = 249.99m,
			CategoryId = categories[0].Id,
			Image = await ReadImageAsync(Path.Combine(imagePath, "erkekgomlek.jpg"))
		},
		new()
		{
			Name = "Erkek Pantalon",
			Description = "Rahat ve şık",
			Price = 900.99m,
			CategoryId = categories[0].Id,
			Image = await ReadImageAsync(Path.Combine(imagePath, "erkekpantalon.jpg"))
		},
		new()
		{
			Name = "Erkek Spor Ayakkabı",
			Description = "Koşuya uygun, rahat",
			Price = 1200.99m,
			CategoryId = categories[0].Id,
			Image = await ReadImageAsync(Path.Combine(imagePath, "erkekayakkabi.jpg"))
		},
		new()
		{
			Name = "Kadın Deri Ceket",
			Description = "Genuine Leather, Siyah",
			Price = 899.99m,
			CategoryId = categories[1].Id,
			Image = await ReadImageAsync(Path.Combine(imagePath, "kadindericeket.jpg"))
		},
		new()
		{
			Name = "Kadın Elbise",
			Description = "Rahat ve şık",
			Price = 700.99m,
			CategoryId = categories[1].Id,
			Image = await ReadImageAsync(Path.Combine(imagePath, "kadinelbise.jpg"))
		},
		new()
		{
			Name = "Kadın Topuklu Ayakkabı",
			Description = "Şık ve rahat yürüme imkanı",
			Price = 2100.99m,
			CategoryId = categories[1].Id,
			Image = await ReadImageAsync(Path.Combine(imagePath, "kadinayakkabi.jpg"))
		},
		new()
		{
			Name = "Çocuk Spor Ayakkabı",
			Description = "Rahat ve hafif tasarım",
			Price = 349.99m,
			CategoryId = categories[2].Id,
			Image = await ReadImageAsync(Path.Combine(imagePath, "cocuksporayakkabi.jpg"))
		}
	};

			// Var olmayanları filtrele (ismi aynıysa atla)
			var existingProductNames = context.Products.Select(p => p.Name).ToHashSet();

			var newProducts = predefinedProducts
				.Where(p => !existingProductNames.Contains(p.Name))
				.ToList();

			if (newProducts.Any())
			{
				await context.Products.AddRangeAsync(newProducts);
				await context.SaveChangesAsync();
			}
		}


		private static async Task<byte[]> ReadImageAsync(string imagePath)
		{
			if (!File.Exists(imagePath))
			{
				// Varsayılan resmi kullan
				imagePath = Path.Combine(Path.GetDirectoryName(imagePath), "../default-product.png");
			}
			return await File.ReadAllBytesAsync(imagePath);
		}
	}
}